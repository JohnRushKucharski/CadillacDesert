namespace Model.Inputs.Functions
{
    public interface IComputableFunction: IFunction
    {
        IComputableFunction Compose(IFunction transformFunction);
    }
}
