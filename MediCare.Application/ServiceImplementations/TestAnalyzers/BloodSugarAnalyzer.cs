using System.Text.RegularExpressions;
using MediCare.Application.DTOs.LabTestDTO;

namespace MediCare.Application.ServiceImplementations.TestAnalyzers
{
    public class BloodSugarAnalyzer : ITestAnalyzer
    {
        public bool CanAnalyze(string text)
            => text.Contains("BLOOD SUGAR", StringComparison.OrdinalIgnoreCase) ||
               text.Contains("FASTING SUGAR", StringComparison.OrdinalIgnoreCase);

        public LabTestAnalysisResponseDTO Analyze(string text)
        {
            // Normalize text
            string cleaned = Regex.Replace(text, @"\s+", " ").ToUpper();

            // Extract numbers immediately before "BLOOD SUGAR"
            var matches = Regex.Matches(cleaned, @"(\d+)\s*BLOOD SUGAR");

            int fastingValue = matches.Count > 0 ? int.Parse(matches[0].Groups[1].Value) : 0;
            int postLunchValue = matches.Count > 1 ? int.Parse(matches[1].Groups[1].Value) : 0;

            // Detect urine and acetone results
            bool urineSugarPresent = cleaned.Contains("PRESENT (+)URINE SUGAR");
            bool acetonePresent = cleaned.Contains("PRESENT ++URINE ACETONE");
            bool urineSugarAbsent = cleaned.Contains("ABSENT URINE SUGAR");
            bool acetoneAbsent = cleaned.Contains("ABSENT URINE ACETONE");

            // Determine abnormality
            bool fastingAbnormal = fastingValue < 70 || fastingValue > 110;
            bool postLunchAbnormal = postLunchValue > 140;
            bool abnormal = fastingAbnormal || postLunchAbnormal || urineSugarPresent || acetonePresent;

            // Summary
            string summary =
                $"Fasting Sugar: {fastingValue} mg/dl (Normal: 70–110)\n" +
                $"2 Hrs After Lunch: {postLunchValue} mg/dl (Normal: <140)\n" +
                $"Urine Sugar: {(urineSugarPresent ? "Present (+)" : urineSugarAbsent ? "Absent" : "Not mentioned")}\n" +
                $"Urine Acetone: {(acetonePresent ? "Present (++)" : acetoneAbsent ? "Absent" : "Not mentioned")}\n";

            // Suggestions
            string suggestion = abnormal
                ? BuildAbnormalSuggestion(fastingAbnormal, postLunchAbnormal, urineSugarPresent, acetonePresent)
                : "All readings appear normal. Maintain healthy habits and routine checkups.";

            return new LabTestAnalysisResponseDTO
            {
                Summary = summary,
                IsAbnormal = abnormal,
                Suggestions = suggestion
            };
        }

        private string BuildAbnormalSuggestion(bool fastingAbnormal, bool postLunchAbnormal, bool urineSugarPresent, bool acetonePresent)
        {
            var messages = new List<string>();
            if (fastingAbnormal) messages.Add("Fasting sugar is outside the normal range.");
            if (postLunchAbnormal) messages.Add("Post-lunch sugar is higher than normal.");
            if (urineSugarPresent) messages.Add("Glucose detected in urine — possible early sign of diabetes.");
            if (acetonePresent) messages.Add("Acetone detected — may indicate uncontrolled sugar levels.");
            messages.Add("Please consult your doctor for further evaluation.");
            return string.Join(" ", messages);
        }
    }
}