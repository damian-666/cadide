﻿using IDE.Core.Interfaces;

namespace IDE.Core.ViewModels
{
    public interface IPropertiesToolWindow : IToolWindow
    {

    }

    /*
    #region Complex Object

    [ExpandableObject]
    public class ComplexObject
    {
        public int ID { get; set; }

        [ExpandableObject]
        public ExpandableObservableCollection<ComplexSubObject> Classes { get; set; }

        public ComplexObject()
        {
            ID = 1;
            Classes = new ExpandableObservableCollection<ComplexSubObject>();
            Classes.Add(new ComplexSubObject() { Name = "CustomFoo" });
            Classes.Add(new ComplexSubObject() { Name = "My Other Foo" });
        }
    }

    [ExpandableObject]
    public class ComplexSubObject
    {
        public string Name { get; set; }

        [ExpandableObject]
        public ExpandableObservableCollection<SimpleValues> Types { get; set; }

        public ComplexSubObject()
        {
            Types = new ExpandableObservableCollection<SimpleValues>();
            Types.Add(new SimpleValues() { name = "foo", value = "bar" });
            Types.Add(new SimpleValues() { name = "bar", value = "foo" });
        }
    }

    public class SimpleValues
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class ExpandableObservableCollection<T> : ObservableCollection<T>,
                                                     ICustomTypeDescriptor
    {
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            // Create a collection object to hold property descriptors
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            for (int i = 0; i < Count; i++)
            {
                pds.Add(new ItemPropertyDescriptor<T>(this, i));
            }

            return pds;
        }

        #region Use default TypeDescriptor stuff

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, noCustomTypeDesc: true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, noCustomTypeDesc: true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, noCustomTypeDesc: true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, noCustomTypeDesc: true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, noCustomTypeDesc: true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, noCustomTypeDesc: true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, noCustomTypeDesc: true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, noCustomTypeDesc: true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, noCustomTypeDesc: true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(this, attributes, noCustomTypeDesc: true);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    public class ItemPropertyDescriptor<T> : PropertyDescriptor
    {
        private readonly ObservableCollection<T> _owner;
        private readonly int _index;

        public ItemPropertyDescriptor(ObservableCollection<T> owner, int index)
          : base("#" + index, null)
        {
            _owner = owner;
            _index = index;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                var attributes = TypeDescriptor.GetAttributes(GetValue(null), false);


                if (!attributes.OfType<ExpandableObjectAttribute>().Any())
                {
                    // copy all the attributes plus an extra one (the
                    // ExpandableObjectAttribute)
                    // this ensures that even if the type of the object itself doesn't have the
                    // ExpandableObjectAttribute, it will still be expandable. 
                    var newAttributes = new Attribute[attributes.Count + 1];
                    attributes.CopyTo(newAttributes, newAttributes.Length - 1);
                    newAttributes[newAttributes.Length - 1] = new ExpandableObjectAttribute();

                    // overwrite the original
                    attributes = new AttributeCollection(newAttributes);
                }

                return attributes;
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return Value;
        }

        private T Value
          => _owner[_index];

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            _owner[_index] = (T)value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
          => _owner.GetType();

        public override bool IsReadOnly
          => false;

        public override Type PropertyType
          => Value?.GetType();
    }

    #endregion Complex Object
    */
}
