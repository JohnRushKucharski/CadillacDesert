namespace Model.Inputs.Functions
{
    #region Notes
    /* Entity Notes: FunctionBase
using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
public abstract class FunctionBase : IFunction
{
    #region Notes
    //This class only exist  because Entity cannot write interface properities. If that functionality is added to unity then Ordinates Function, Frequency Function etc. could just implement IFunction, and Decorator could be composed of IFunction.
    #endregion

    #region Properties
    public int ProjectId { get; set; }

    public virtual Project Project { get; set; }
    public int Id { get; protected set; }
    public abstract bool IsValid { get; protected set; }
    public abstract FunctionType Type { get; set; }
    #endregion

    #region Methods
    public abstract bool Validate();
    public abstract ICollection<string> ReportValidationErrors();
    public abstract double GetXfromY(double y);

    public void Write()
    {
        string dbPath = String.Format("Data Source={0};Version=3;", Project.Instance.GetFilePathWithoutExtension() + ".sqlite");
        var sqLiteConnection = new System.Data.SQLite.SQLiteConnection(dbPath);
        using (var context = new DataBase.DataContext(sqLiteConnection, false))
        {
            //context.Functions.Add(this);
            context.SaveChanges();
        }
    }
    #endregion
}
}
*/
    #endregion

    public interface IFunction : IValidateData
    {
        FunctionType Type { get; }
        double GetXfromY(double y);
    }



}
