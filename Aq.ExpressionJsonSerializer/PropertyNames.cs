namespace Aq.ExpressionJsonSerializer
{
    public class PropertyNames
    {
        public PropertyNames()
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                prop.SetValue(this, prop.Name);
            }
        }

        public string Argument { get; set; }
        public string Arguments { get; set; }
        public string AssemblyName { get; set; }
        public string Body { get; set; }
        public string Constructor { get; set; }
        public string Conversion { get; set; }
        public string ElementType { get; set; }
        public string Expression { get; set; }
        public string Expressions { get; set; }
        public string Generic { get; set; }
        public string GenericArguments { get; set; }
        public string IfFalse { get; set; }
        public string IfTrue { get; set; }
        public string Indexer { get; set; }
        public string Left { get; set; }
        public string LiftToNull { get; set; }
        public string Member { get; set; }
        public string Members { get; set; }
        public string MemberType { get; set; }
        public string Method { get; set; }
        public string Name { get; set; }
        public string NodeType { get; set; }
        public string Object { get; set; }
        public string Operand { get; set; }
        public string Parameters { get; set; }
        public string Right { get; set; }
        public string Signature { get; set; }
        public string TailCall { get; set; }
        public string Test { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string TypeOperand { get; set; }
        public string Value { get; set; }
        public string Variables { get; set; }
    }
}