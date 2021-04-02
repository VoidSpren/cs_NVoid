using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace cs_NVoid
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            //Console.Write(">>> ");
            //var input = Console.ReadLine();
            //var input = "LET a = 0\nWHILE a < 5 REPEAT\n     PRINT a\n       a = a + 1\nENDWHILE";
            var input = File.ReadAllText("basic_program.txt");

            for(int i = 0; i < input.Length; i++){
                if(input[i]  == '\r'){
                    input = input.Remove(i,1);
                    i--;
                }
            }

            var lexer = new Lexer();

            // foreach(var i in lexer.lexText(input))
            //     Console.WriteLine(i);

            var parser = new Parser();
            var emmiter = new Emmiter();

            var program = parser.parseTokens(lexer.lexText(input));
            var compiled = emmiter.emit(program, parser.symbols);
            
            Console.WriteLine();
            foreach(var stat in program)
                Console.WriteLine(stat);
            Console.WriteLine("\n\n");

            Console.Write(compiled);

            File.WriteAllText("c_compiled.c", compiled);

            // foreach(var i in parser.parseTokens(lexer.lexText(input))){
            //     Console.WriteLine(i.ToString());
            // }
            // foreach(var i in lexer.lexText(input)){
            //     Console.WriteLine(i.type + " " + i.txt);
            // }
        }
    }

    enum TType{
        EOF = -1, NEWLINE, IDENT,
        NUM, STRING,
        LABEL, GOTO, PRINT, INPUT, LET, IF, THEN, ENDIF, WHILE, REPEAT, ENDWHILE,
        EQ, PLUS, MINUS, AST, SLASH, EQEQ, NOTEQ, LT, LTEQ, GT, GTEQ,
        LPARENTHESIS, RPARENTHESIS,
        INVALID
    }



    enum BType{
        STAT, COMP, EXPR, TERM, UNARY, PRIM, IDENT, NUM, NL
    }
    
}