using System;
using System.Collections.Generic;

namespace SPLConqueror_Core
{
    public class Grammar
    {

        private readonly FMTree VMTree;

        private Dictionary<string, List<string>> grammar = new Dictionary<string, List<string>>();

        private List<string> terminals = new List<string>();

        public List<string> Terminals { get => terminals; set => terminals = value; }

        public Grammar(FMTree tree)
        {
            VMTree = tree;
            generateGrammar();

        }

        public List<string> GetRule(string Terminal)
        {
            foreach (List<string> rule in grammar.Values)
            {
                if (rule.Contains(Terminal)) return rule;
            }
            throw new InvalidOperationException("Terminnal was not in contained in a rule");
        }

        private void generateGrammar()
        {
            List<string> rules = new List<string>();
            string rule = "";
            FMNode root = VMTree.Root;
            if (root.Children.Count > 0)
            {
                foreach (FMNode child in root.Children)
                {
                    rule += "<" + child.Name + ">";
                    // TODO check if this is the right way to go
                    if (child.ExcludedOptions.Count > 0)
                    {
                        rules.Add(rule);
                        rule = "";
                    }
                }
                if (rule != "")
                {
                    rules.Add(rule);
                }
                grammar.Add("<" + root.Name + ">", rules);
                foreach (FMNode child in root.Children)
                {
                    generateGrammar(child);
                }

            }
            else
            {
                throw new InvalidOperationException();
            }

        }

        private void generateGrammar(FMNode node)
        {
            List<string> rules = new List<string>();
            string rule = "";
            bool leafLevel = node.Children.TrueForAll(x => x.Children.Count == 0);
            bool isLeaf = node.Children.Count == 0;
            if (isLeaf)
            {
                bool optional = checkOptional(node);
                if (optional)
                {
                    rules.Add("\u03b5");
                }
                rule += "" + node.Name + "";
                Terminals.Add(node.Name);

            }
            else
            {
                if (leafLevel)
                {
                    foreach (FMNode child in node.Children)
                    {
                        bool optional = checkOptional(node);
                        if (optional && !rules.Contains("\u03b5"))
                        {
                            rules.Add("\u03B5");
                        }
                        rule += "" + child.Name + "";
                        // TODO check if this is the right way to go
                        if (child.ExcludedOptions.Count > 0)
                        {
                            rules.Add(rule);
                            rule = "";
                        }
                        Terminals.Add(child.Name);
                    }
                }
                else
                {
                    foreach (FMNode child in node.Children)
                    {
                        rule += "<" + child.Name + ">";
                        // TODO check if this is the right way to go
                        if (child.ExcludedOptions.Count > 0)
                        {
                            rules.Add(rule);
                            rule = "";
                        }
                    }
                }
            }
            if (rule != "")
            {
                rules.Add(rule);
            }
            grammar.Add("<" + node.Name + ">", rules);
            foreach (FMNode child in node.Children)
            {
                if (child.Children.Count > 0) generateGrammar(child);
            }

        }

        private bool checkOptional(FMNode node)
        {
            if (node.Optional) return true;
            else if (node.Parent != null) return false;
            else return checkOptional(node.Parent);
        }

        public override string ToString ()
        {
            string result = "";
            foreach (KeyValuePair<string, List<string>> obj in grammar)
            {
                result += obj.Key + " ::= " + String.Join(" | ", obj.Value) + "\n";
            }
            return result;
        }

        public void print()
        {
            Console.WriteLine(this.ToString());
            Console.WriteLine("Terminals: " + String.Join(", ", Terminals));
        }

    }
}
