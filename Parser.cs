using System.Collections.Generic;
using System;

namespace cs_NVoid{
    class Parser{
        private List<Token> tokens = null;
        public HashSet<string> created_labels{ get; }
        public HashSet<string> goto_labels{ get; }
        public HashSet<string> symbols{ get; }
        private int pos = 0;
        public Token current{
            get{
                return pos < tokens.Count? tokens[pos]: tokens[tokens.Count - 1];
            }
        }

        public Parser() {
            created_labels = new HashSet<string>();
            goto_labels = new HashSet<string>();
            symbols = new HashSet<string>();
        }

        public List<Statement> parseTokens(List<Token> args){
            tokens = args;
            pos = 0;

            var program = new List<Statement>();
            if(tokens != null){
                while(current.type == TType.NEWLINE) pos++;
                while(current.type != TType.EOF){
                    program.Add(new Statement(this));
                }
            }

            foreach(var goto_lab in goto_labels){
                if(!created_labels.Contains(goto_lab)){
                    throw new Exception("Label " + goto_lab + " not declared");
                }
            }

            return program;
        }

        public bool match(TType type){
            if(current.type != type){
                if(current.type == TType.EOF && type == TType.NEWLINE && pos >= tokens.Count - 1){
                    return true;
                }
                throw new Exception("Expected: "+ type + ", got: " + current.type);
            }
            return true;
        }

        public Token peek(int offset){
            if(pos + offset >= tokens.Count){
                return tokens[tokens.Count - 1];
            }
            if(pos + offset < 0){
                return null;
            }
            return tokens[pos + offset];
        }
        public Token nextToken(){
            pos++;
            return current;
        }

        public bool validateSet(TType type, string name){
            if(type == TType.GOTO){
                goto_labels.Add(name);
            }else if(type == TType.LABEL){
                if(created_labels.Contains(name)){
                    throw new Exception("label " + name + " already declared");
                }
                created_labels.Add(name);
            }else if(type == TType.LET || type == TType.INPUT){
                if(created_labels.Contains(name)){
                    throw new Exception("name " + name + " already declared as a label");
                }
                symbols.Add(name);
            }
            return true;
        }

        public bool validateGet(string name){
            if(created_labels.Contains(name)){
                throw new Exception("name " + name + " already declared as label, symbol expected");
            }

            return symbols.Contains(name);
        }
    }

}