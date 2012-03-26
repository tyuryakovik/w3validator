using System;
using System.Collections.Generic;
using System.Reflection;

[assembly: CLSCompliant(true)]
namespace KBProject.Common
{
  /// <summary>
  /// Attribute to skip field copying.</summary>
  /// <remarks>
  /// Set this attribute for a field that should  
  /// not be copied to clone object and 
  /// retain default value instead. Default value
  /// is the one that is set by parameterless constructor.</remarks>
  [AttributeUsage(AttributeTargets.Field)]
  public sealed class SkipFieldAttribute : Attribute { }

  /// <summary>
  /// Inherit this class to have DeepCopy functionality needed
  /// for Prototype design pattern.</summary>
  /// <remarks>
  /// DeepCopy clones the class and all of its substructure 
  /// consisting of Prototype subclasses. 
  /// Circular references and interconnecting tree structure 
  /// are supported by this method.
  /// Array fields (Prototype[]) are supported.
  /// To support other type of containers, override ExtraCopy
  /// in your subclass.
  /// All fields that are not inherited from Prototype will be
  /// copied as-is. I.e. reference fields will be pointing at 
  /// the same objects as the source object fields.
  ///</remarks>
  public abstract class Prototype
  {
    /// <summary>
    /// Counts DeepCopy operations. </summary>
    private static ulong copyIndex;

    /// <summary>
    /// Used to remember created copy reference</summary>
    /// <remarks>
    /// When copy operation encounters the object that is
    /// already copied on previous iterations, it uses this
    /// reference, instead of creating new object. As a result,
    /// copied structure matches exactly.</remarks>
    [SkipFieldAttribute]
    private object currentCopy;

    /// <summary>
    /// Indicates copy operation when currentCopy was created.</summary>
    private ulong currentIndex;
    
    /// <summary>
    /// Parameterless empty constructor
    /// </summary>
    protected Prototype()
    {
    }

    /// <summary>
    /// Call this to make a deep copy of the objects structure.</summary>
    /// <returns>
    /// Returns copied object structure. User is responsible for casting.</returns>
    public object DeepCopy()
    {
      return PerformCopy(++copyIndex);
    }

    /// <summary>
    /// Recursively copies objects.</summary>
    /// <param name="index"> 
    /// index of copy operation in progress</param>
    /// <returns>
    /// Returns copied object.</returns>
    private object PerformCopy(ulong index)
    {
      if (currentIndex == index)
        return currentCopy;
      currentIndex = index;
      currentCopy = Activator.CreateInstance(this.GetType());
      foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        if (field.GetCustomAttributes(typeof(SkipFieldAttribute), false).Length == 0)
          field.SetValue(currentCopy, ArrayCopy(field.GetValue(this)));
      ExtraCopy(currentCopy);
      return currentCopy;
    }

    /// <summary>
    /// Does a recursive PerformCopy call for Prototype[] array fields.</summary>
    /// <param name="value"> 
    /// Value of source object field.</param>
    /// <returns>
    /// If input value is Prototype[] array, returns deep Copy of it. 
    /// Otherwise returns input value unchanged.</returns>
    private object ArrayCopy(object value)
    {
      Prototype[] inputArray = value as Prototype[];
      if (inputArray == null)
        return ProtoCopy(value);
      object[] outputArray = (object[])Activator.CreateInstance(value.GetType(), new object[] { inputArray.Length });
      for (int i = 0; i < inputArray.Length; i++)
        outputArray[i] = ProtoCopy(inputArray[i]);      
      return outputArray;
    }

    /// <summary>
    /// Does a recursive PerformCopy call for Prototype fields.</summary>
    /// <param name="value"> 
    /// Value of source object field.</param>
    /// <returns>
    /// If input value is Prototype, returns deep Copy of it. 
    /// Otherwise returns input value unchanged.</returns>
    protected object ProtoCopy(object value)
    {
      Prototype protoValue = value as Prototype;
      if (protoValue != null)
        return protoValue.PerformCopy(currentIndex);
      else
        return value;
    }

    /// <summary>
    /// Override this method to support additional container types.</summary>
    /// <remarks>
    /// Example:
    ///     public class subProto : Prototype
    ///     {
    ///       List%subProto% branches = new List%subProto%();
    ///
    ///       protected override void ExtraCopy(object copy)
    ///       {
    ///         subProto subcopy = Copy as subProto;
    ///         subcopy.branches = new List%subProto%();
    ///         foreach (subProto item in branches)
    ///           if (item != null)
    ///             subcopy.branches.Add(ProtoCopy(item) as subProto);
    ///       }
    ///     } 
    ///</remarks>
    /// <param name="copy"> 
    /// Copied object.</param>
    protected virtual void ExtraCopy(object copy)
    { }

  }

}
