using System;

namespace cs_NVoid{
    class Statement : Branch{
        public TType stat_type{ get; }
        public string print_txt{ get; }
        public Statement(Parser parser): base(BType.STAT) {
            stat_type = parser.current.type;
            var index = 0;
            switch(stat_type){
                    case TType.LABEL:
                        parser.nextToken();
                        branches.Add(new Ident(parser, false));
                        
                        parser.validateSet(TType.LABEL, ((Ident)branches[0]).name);

                        emmiter_txt = branches[0].getEmmiter_txt() + ":\n";

                        branches.Add(new NewLine(parser));
                        break;
                    case TType.GOTO:
                        parser.nextToken();
                        branches.Add(new Ident(parser, false));
                        
                        parser.validateSet(TType.GOTO, ((Ident)branches[0]).name);

                        emmiter_txt = "goto " + branches[0].getEmmiter_txt() + ";\n";

                        branches.Add(new NewLine(parser));
                        break;
                    case TType.PRINT:
                        emmiter_txt = "printf(\"";
                        if(parser.nextToken().type == TType.STRING){
                            print_txt = parser.current.txt;
                            parser.nextToken();
                            emmiter_txt += print_txt + "\\n\");\n";
                        }else{
                            branches.Add(new Expression(parser));
                            emmiter_txt += "%.2f\\n\", (float)(" + branches[0].getEmmiter_txt() + "));\n";
                        }
                        branches.Add(new NewLine(parser));

                        break;
                    case TType.INPUT:
                        parser.nextToken();
                        branches.Add(new Ident(parser, false));
                        
                        parser.validateSet(TType.INPUT, ((Ident)branches[0]).name);

                        var name = branches[0].getEmmiter_txt();

                        emmiter_txt = "if(0 == scanf(\"%f\", &" + name + ")) {\n";
                        emmiter_txt += name + " = 0;\n";
                        emmiter_txt += "scanf(\"%*s\");\n}\n";

                        branches.Add(new NewLine(parser));
                        break;
                    case TType.LET:
                        parser.nextToken();
                        branches.Add(new Ident(parser, false));
                        
                        parser.validateSet(TType.LET, ((Ident)branches[0]).name);

                        parser.match(TType.EQ);

                        emmiter_txt = branches[0].getEmmiter_txt() + " = ";

                        parser.nextToken();
                        branches.Add(new Expression(parser));

                        emmiter_txt += branches[1].getEmmiter_txt() + ";\n";

                        branches.Add(new NewLine(parser));
                        break;
                    case TType.IF:
                        parser.nextToken();
                        branches.Add(new Comparison(parser));

                        emmiter_txt = "if(" + branches[0].getEmmiter_txt();

                        parser.match(TType.THEN);

                        emmiter_txt += "){\n";

                        parser.nextToken();
                        branches.Add(new NewLine(parser));

                        index = 1;
                        while(parser.current.type != TType.ENDIF){
                            branches.Add(new Statement(parser));
                            index++;
                            emmiter_txt += branches[index].getEmmiter_txt();
                        }

                        parser.match(TType.ENDIF);

                        emmiter_txt += "}\n";

                        parser.nextToken();
                        branches.Add(new NewLine(parser)); 
                        break;
                    case TType.WHILE:
                        parser.nextToken();
                        branches.Add(new Comparison(parser));

                        emmiter_txt = "while(" + branches[0].getEmmiter_txt();

                        parser.match(TType.REPEAT);

                        emmiter_txt += "){\n";
                        
                        parser.nextToken();
                        branches.Add(new NewLine(parser));

                        index = 1;
                        while(parser.current.type != TType.ENDWHILE){
                            branches.Add(new Statement(parser));
                            index++;
                            emmiter_txt += branches[index].getEmmiter_txt();
                        }

                        parser.match(TType.ENDWHILE);

                        emmiter_txt += "}\n";

                        parser.nextToken();
                        branches.Add(new NewLine(parser));
                        break;
                    default:
                        throw new Exception("expected statement, got " + stat_type);
            }
        }

        public override string ToString()
        {
            var str = bType + " ["+ stat_type +"]" + " {";
            foreach(var i in branches){
                str += i.ToString();
            }
            str += "} ";

            return str; 
        }
    }
}