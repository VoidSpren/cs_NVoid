using System.Collections.Generic;

namespace cs_NVoid{
    class Lexer{
        private string txt = "";
        private int pos = 0;
        private char current{
            get{
                if(pos >= txt.Length)
                    return '\0';
                return txt[pos];
            }
        }
        private List<Token> tokens = new List<Token>();
        public Lexer(){}

        public List<Token> lexText(string arg){ 
            txt = arg.Trim();
            pos = 0;
            tokens.Clear();

            if(!string.IsNullOrEmpty(txt)){
                Token token;
                do{
                    token = nextToken();
                    tokens.Add(token);
                }while(token.type != TType.EOF);
            }

            return tokens;
        }

        private Token nextToken(){

            skipWhiteSpace();
            skipComment();

            if(char.IsDigit(current)){
                var is_float = false;
                int end;

                var begin = pos;
                while(char.IsDigit(current)) pos++;
                if(current == '.'){
                    is_float = true;

                    if(!char.IsDigit(peek(1))){
                        end = pos - begin;
                        return new Token(pos++, txt.Substring(begin, end), TType.INVALID, "invalid number expression");
                    }

                    pos++;
                    while(char.IsDigit(current)) pos++;
                }

                end = pos - begin;
                var temp_txt = txt.Substring(begin, end);
                
                int value_int = 0;
                float value_float = 0;

                if(is_float){
                    value_float = float.Parse(temp_txt);
                }else{
                    value_int = int.Parse(temp_txt);
                }

                return new Token(begin, temp_txt, TType.NUM, is_float? value_float: value_int);
            }

            if(char.IsLetter(current)){
                var begin = pos++;
                while(char.IsLetter(current))pos++;
                
                var sub_txt = txt.Substring(begin, pos - begin);
                //LABEL, GOTO, PRINT, INPUT, LET, IF, THEN, ENDIF, WHILE, REPEAT, ENDWHILE
                switch(sub_txt){
                    case "LABEL":
                        return new Token(begin, sub_txt, TType.LABEL, null);
                    case "GOTO":
                        return new Token(begin, sub_txt, TType.GOTO, null);
                    case "PRINT":
                        return new Token(begin, sub_txt, TType.PRINT, null);
                    case "INPUT":
                        return new Token(begin, sub_txt, TType.INPUT, null);
                    case "LET":
                        return new Token(begin, sub_txt, TType.LET, null);
                    case "IF":
                        return new Token(begin, sub_txt, TType.IF, null);
                    case "THEN":
                        return new Token(begin, sub_txt, TType.THEN, null);
                    case "ENDIF":
                        return new Token(begin, sub_txt, TType.ENDIF, null);
                    case "WHILE":
                        return new Token(begin, sub_txt, TType.WHILE, null);
                    case "REPEAT":
                        return new Token(begin, sub_txt, TType.REPEAT, null);
                    case "ENDWHILE":
                        return new Token(begin, sub_txt, TType.ENDWHILE, null);
                }
                return new Token(begin, sub_txt, TType.IDENT, null);
            }

            switch(current){
                case '\0':
                    return new Token(pos++, "\0", TType.EOF, null);
                case '\n':
                    return new Token(pos++, "\\n", TType.NEWLINE, null);
                case '=':
                    if(peek(1) == '=') {
                        var temp_pos = pos++; pos++;
                        return new Token(temp_pos, "==", TType.EQEQ, null);
                    }
                    return new Token(pos++, "=", TType.EQ, null);
                case '+':
                    return new Token(pos++, "+", TType.PLUS, null);
                case '-':
                    return new Token(pos++, "-", TType.MINUS, null);
                case '*':
                    return new Token(pos++, "*", TType.AST, null);
                case '/':
                    return new Token(pos++, "/", TType.SLASH, null);
                case '!':
                    if(peek(1) == '=') {
                        var temp_pos = pos++; pos++;
                        return new Token(temp_pos, "!=", TType.NOTEQ, null);
                    }
                    return new Token(pos++, "!", TType.INVALID, "Invalid: expected != got !");
                case '<':
                    if(peek(1) == '=') {
                        var temp_pos = pos++; pos++;
                        return new Token(temp_pos, "<=", TType.LTEQ, null);
                    }
                    return new Token(pos++, "<", TType.LT, null);
                case '>':
                    if(peek(1) == '=') {
                        var temp_pos = pos++; pos++;
                        return new Token(temp_pos, ">=", TType.GTEQ, null);
                    }
                    return new Token(pos++, ">", TType.GT, null);
                case '(':
                    return new Token(pos++, "(", TType.LPARENTHESIS, null);
                case ')':
                    return new Token(pos++, ")", TType.RPARENTHESIS, null);
                case '\"':
                    pos++;
                    var begin = pos;

                    while (current != '\"'){
                        if(current == '\r' || current == '\n' || current == '\t' || current == '\n' || current == '\\'){
                            while (current != '\"' || current != '\n') pos++;
                            return new Token(pos, txt.Substring(begin, pos++), TType.INVALID, "invalid charachter in string");
                        }
                        pos++;
                    }

                    var end = pos - begin;
                    var sub_txt = txt.Substring(begin, end);
                    return new Token(pos++, sub_txt, TType.STRING, null);
            }
            
            return new Token(pos++, peek(-1) + "", TType.INVALID, "Invalid token");
        }

        private char peek(int offset){
            if(pos + offset >= txt.Length || pos + offset < 0) return '\0';
            return txt[pos + offset];
        }

        private void skipWhiteSpace(){
            while(current == ' ' || current == '\r' || current == '\t') pos++;
        }

        private void skipComment(){
            if(current == '#'){
                while(current != '\n') pos++;
                pos++;
            }
        }
    }
}