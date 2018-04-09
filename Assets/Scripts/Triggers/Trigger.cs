public class Trigger {

    public enum Type {
        FIRST,
        RANDOM,
        EVENT,
        OVERDOSE,
        OVERNIGHT
    }

    public Type type;

    // For use in Type.EVENT and Type.OVERDOSE triggers
    public virtual bool CheckValid(Slug slug, Drug.DrugState state) {
        return true;
    }
}
