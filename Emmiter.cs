using System.Collections.Generic;

namespace cs_NVoid{
    class Emmiter{
        public string file_path{ get; }
        public Emmiter(/*string path*/){
            //file_path = path;
        }

        public string emit(List<Statement> arg, HashSet<string> symbols){
            var str = "#include <stdio.h>\nint main(){\n";

            foreach(var name in symbols){
                str += "float " + name + ";\n";
            }

            foreach(var stat in arg){
                str += stat.getEmmiter_txt();
            }

            str += "return 0;\n}\n";

            return str;
        }
    }
}