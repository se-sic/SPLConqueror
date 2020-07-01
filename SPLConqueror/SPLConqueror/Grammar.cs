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
            print();
            throw new InvalidOperationException("Terminnal was not contained in a rule " + Terminal);
        }

        private void generateGrammar()
        {
            FMNode root = VMTree.Root;
            if (root.Children.Count > 0)
            {
                generateGrammar(root);
            }
            else
            {   
                Terminals.Add(root.Name);
                throw new InvalidOperationException();
            }

        }

        private void generateGrammar(FMNode node)
        {
            List<string> rules = new List<string>();
            string rule = "";
            foreach (FMNode child in node.Children)
            {

                if (child.Children.Count > 0)
                {
                    rule += "<" + child.Name + ">";
                    if (child.ExcludedOptions.Count > 0)
                    {
                        rules.Add(rule);
                        rule = "";
                    }
                } 
                else
                { 
                    if (checkOptional(child))
                    {
                        rule += "<" + child.Name + ">";
                        List<string> tmpList = new List<string>();
                        tmpList.Add("\u03B5");
                        tmpList.Add(child.Name);
                        grammar.Add("<" + child.Name + ">", tmpList);
                    }
                    else
                    { 
                        if (child.ExcludedOptions.Count > 0)
                        {
                            if (node.Children.TrueForAll(x => x.Children.Count == 0))
                            {
                                rule += "" + child.Name + "";
                                rules.Add(rule);
                                rule = "";
                            }
                            else 
                            { 

                                rule += "<" + child.Name + ">";
                                rules.Add(rule);
                                rule = "";
                                List<string> tmpList = new List<string>();
                                tmpList.Add(child.Name);
                                grammar.Add("<" + child.Name + ">", tmpList);
                            }
                        }
                        else 
                        { 
                            rule += "<" + child.Name + ">";
                            List<string> tmpList = new List<string>();
                            tmpList.Add(child.Name);
                            grammar.Add("<" + child.Name + ">", tmpList);
                        }
                    }
                    Terminals.Add(child.Name);
                }
            }
            if (rule != "")
            {
                rules.Add(rule);
            }
            if (!grammar.ContainsKey("<" + node.Name + ">")) { 
                grammar.Add("<" + node.Name + ">", rules);
            }
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
