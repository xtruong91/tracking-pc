using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AutoTest.Models
{
  public abstract class BaseModel
  {
    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
    {
      //PropertyChanged(this, new PropertyChangedEventArgs(ExpressionHelper.Name(expression)));
    }

    protected virtual void SetPropertyAndNotify<T>(ref T currentValue, T newValue, Expression<Func<T>> propertyExpression)
    {
      if (Equals(currentValue, newValue))
      {
        return;
      }

      currentValue = newValue;
      RaisePropertyChanged(propertyExpression);
    }
  }
}
