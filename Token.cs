namespace cs_NVoid{
    class Token{
    public int pos { get; set;}
    public string txt { get; set;}
    public TType type { get; set;}
    public object value { get; set;}

    public Token(int pos, string txt, TType type, object value){
        this.pos = pos; this.txt = txt; this.type = type; this.value = value;
    }
    public Token(){}

    public override string ToString()
    {
        return "["+pos+" "+type+" "+txt+"]";
    }
}
}