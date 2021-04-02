namespace cs_NVoid{
    class Comparison : Branch{

        public List<string> comp_Ops{ get; }
        public Comparison(Parser parser): base(BType.COMP){
            comp_Ops = new List<string>();
            branches.Add(new Expression(parser));

            emmiter_txt = branches[0].getEmmiter_txt();

            var type = parser.current.type;
            if(type == TType.EQEQ ||type == TType.LTEQ ||type == TType.GTEQ ||
            type == TType.NOTEQ ||type == TType.LT ||type == TType.GT){
                comp_Ops.Add(parser.current.txt);
                parser.nextToken();
                branches.Add(new Expression(parser));

                emmiter_txt += " " + comp_Ops[0] + " " + branches[1].getEmmiter_txt();
            }else{
                throw new Exception("Expected a comparison operator, got " + type);
            }

            // var comp_i = 0;
            // var branch_i = 1;
            // type = parser.current.type;
            // while(type == TType.EQEQ ||type == TType.LTEQ ||type == TType.GTEQ ||
            //    type == TType.NOTEQ ||type == TType.LT ||type == TType.GT){
            //        comp_Ops.Add(parser.current.txt);
            //        parser.nextToken();
            //        branches.Add(new Expression(parser));

            //        comp_i++; branch_i++;
            //        emmiter_txt += 

            //        type = parser.current.type;
            // }
        }

        public override string ToString()
        {
            var str = bType + " [ ";
            
            foreach(var i in comp_Ops){
                str += i + " ";
            }
            str += "]" + " {";

            foreach(var i in branches){
                str += i.ToString();
            }
            str += "} ";

            return str; 
        }
    }

    class Expression : Branch{
        public string operations { get; }
        public Expression(Parser parser): base(BType.EXPR){
            operations = "";
            branches.Add(new Term(parser));

            emmiter_txt = branches[0].getEmmiter_txt();

            var op_i = 0;
            var branch_i = 1;
            while(parser.current.type == TType.PLUS || parser.current.type == TType.MINUS){
                operations += parser.current.type == TType.PLUS? "+": "-";
                parser.nextToken();
                branches.Add(new Term(parser));

                emmiter_txt += " " + operations[op_i] + " " + branches[branch_i].getEmmiter_txt();
                op_i++; branch_i++;
            }
        }

        public override string ToString()
        {
            var str = bType + " [ ";
            
            foreach(var i in operations){
                str += i + " ";
            }
            str += "]" + " {";

            foreach(var i in branches){
                str += i.ToString();
            }
            str += "} ";

            return str; 
        }
    }

    class Term : Branch{
        public string operations{ get; }
        public Term(Parser parser): base(BType.TERM){
            operations = "";
            branches.Add(new Unary(parser));

            emmiter_txt = branches[0].getEmmiter_txt();

            var op_i = 0;
            var branch_i = 1;
            while(parser.current.type == TType.AST || parser.current.type == TType.SLASH){
                operations += parser.current.type == TType.AST? "*": "/";
                parser.nextToken();
                branches.Add(new Unary(parser));

                emmiter_txt += " " + operations[op_i] + " " + branches[branch_i].getEmmiter_txt();
                op_i++; branch_i++;
            }
        }

        public override string ToString()
        {
            var str = bType + " [ ";
            
            foreach(var i in operations){
                str += i + " ";
            }
            str += "]" + " {";

            foreach(var i in branches){
                str += i.ToString();
            }
            str += "} ";

            return str; 
        }
    }

    class Unary : Branch{
        public Unary(Parser parser): base(BType.UNARY){
            var neg = parser.current.type == TType.MINUS;
            if(neg || parser.current.type == TType.PLUS)
                parser.nextToken();

            branches.Add(new Primary(parser, neg));

            emmiter_txt = branches[0].getEmmiter_txt();
        }
    }

    class Primary : Branch{
        public Primary(Parser parser, bool neg): base(BType.PRIM){
            if(parser.current.type == TType.NUM){
                branches.Add(new Number(parser, neg));
            }else if(parser.current.type == TType.IDENT){
                branches.Add(new Ident(parser, neg));
                if(!parser.validateGet(((Ident)branches[0]).name)){
                    throw new Exception("Symbol " + ((Ident)branches[0]).name + " not declared");
                }
            }else{
                throw new Exception("Expected NUM/IDENT, got " + parser.current.type);
            }

            emmiter_txt = branches[0].getEmmiter_txt();
        }
    }

    class Ident : Branch{
        public string name{ get; }
        public bool is_neg{ get; }
        public Ident(Parser parser, bool neg): base(BType.IDENT){
            name = parser.current.txt;
            is_neg = neg;
            parser.nextToken();

            emmiter_txt = (is_neg? "-": "") + name;
        }

        public override string ToString()
        {
            return bType + " { " + (is_neg? "-":"") + name + " } ";
        }
    }

    class Number : Branch{
        public float num{ get; }
        public bool is_neg{ get; }

        public Number(Parser parser, bool neg): base(BType.NUM){
            is_neg = neg;

            num = (float)parser.current.value;

            emmiter_txt = is_neg? "-": "";
            if(parser.current.value is int){
                emmiter_txt += ((int)num).ToString();
            }else if(parser.current.value is float){
                emmiter_txt += num.ToString();
            }

            parser.nextToken();
        }

        public override string ToString()
        {
            return bType + " { " + (is_neg? "-":"") + num + " } ";
        }
    } 

    class NewLine : Branch{
        public NewLine(Parser parser): base(BType.NL){
            if(parser.match(TType.NEWLINE)){}
            while(parser.nextToken().type == TType.NEWLINE){}
        }
    }
}