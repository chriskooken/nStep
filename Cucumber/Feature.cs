using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace Cucumber
{
    public class Feature
    {
        IList<Scenario> scenarios;
        Scenario background;
        IList<string> summaryLines;
        
        public Feature()
        {
            scenarios = new List<Scenario>();
            summaryLines = new List<string>();
            background = new Scenario();
        }

        public void LoadAndParseFeatureFile(string fileName)
        {
            string line;
            StreamReader SR = File.OpenText(fileName);
            
            var fullText = SR.ReadToEnd();
            SR.Close();

            var items = fullText.Split(new string[] {"Feature:","Background:","Scenario:","Scenario Outline:"}, StringSplitOptions.RemoveEmptyEntries);

            IList<string> types = new List<string>();
            SR = File.OpenText(fileName);
            line = SR.ReadLine();
            while (line != null)
            {
                if (line.Contains("Feature:"))
                    types.Add("Feature:");
                if (line.Contains("Background:"))
                    types.Add("Background:");
                if (line.Contains("Scenario:"))
                    types.Add("Scenario:");
                if (line.Contains("Scenario Outline:"))
                    types.Add("Scenario Outline:");

                line = SR.ReadLine();
            }
            SR.Close();

            IList<NameValue> nv = new List<NameValue>();
            for (int i=0;i<items.Length;i++)
            {
                nv.Add(new NameValue { Value = items[i], Name = types[i] });
            }

            foreach (var nameValue in nv)
            {
                if (nameValue.Name == "Feature:")
                {
                    var lines = nameValue.Value.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in lines)
                    {
                        summaryLines.Add(s.Trim());
                    }
                }

                if (nameValue.Name == "Background:")
                {
                    var lines = nameValue.Value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in lines)
                    {
                        background.Steps.Add(s.Trim());
                    }
                }

                if (nameValue.Name == "Scenario:")
                {
                    var lines = nameValue.Value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var scenario = new Scenario();
                    scenario.Title = lines.First().Trim();
                    foreach (var s in lines)
                    {
                        if (s != lines.First())
                            scenario.Steps.Add(s.Trim());
                    }
                    scenarios.Add(scenario);
                }

                if (nameValue.Name == "Scenario Outline:")
                {
                    var lines = nameValue.Value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var scenario = new Scenario();
                    scenario.Title = lines.First().Trim();
                    foreach (var s in lines)
                    {
                        if (s != lines.First())
                            scenario.Steps.Add(s.Trim());
                    }
                    scenarios.Add(scenario);
                }
            }
        }

        public IList<string> SummaryLines
        { get { return summaryLines; } }  
        
        public Scenario Background
        { get { return background; } }

        public IList<Scenario> Scenarios
        { get { return scenarios; } }
    }
}
