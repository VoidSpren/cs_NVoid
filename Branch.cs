using System.Collections.Generic;

namespace cs_NVoid{
    abstract class Branch{
        public BType bType;
        public List<Branch> branches = new List<Branch>();
        protected string emmiter_txt;

        public Branch(BType arg){
            bType = arg;
        }

        public string getEmmiter_txt(){
            return emmiter_txt;
        }

        public override string ToString()
        {
            var str = bType + " {";
            foreach(var i in branches){
                str += i.ToString();
            }
            str += "} ";

            return str; 
        }
    }
}