using System.Reflection;
using System.Reflection.Emit;

namespace Parser.Utils.SqlQuery
{
    public class Accessor
    {
        protected delegate void SetValueHandler(object component, object value);
        protected delegate object GetValueHandler(object component);

        private SetValueHandler setValueHandler;
        private GetValueHandler getValueHandler;

        public Accessor(Type ownerType, string propertyName)
        {
            PropertyInfo propertyInfo = ownerType.GetProperty(propertyName);

            if (propertyInfo.CanRead)
                this.getValueHandler = this.CreateGetValueHandler(propertyInfo);

            if (propertyInfo.CanWrite)
                this.setValueHandler = this.CreateSetValueHandler(propertyInfo);
        }

        public Accessor(PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead)
                this.getValueHandler = this.CreateGetValueHandler(propertyInfo);

            if (propertyInfo.CanWrite)
                this.setValueHandler = this.CreateSetValueHandler(propertyInfo);
        }

        public object GetValue(object component)
        {
            if (this.getValueHandler == null)
                throw new InvalidOperationException();
            return this.getValueHandler(component);
        }

        public void SetValue(object component, object value)
        {
            if (this.setValueHandler == null)
                throw new InvalidOperationException($"setValueHandler is NULL for component \"{component?.ToString()}\" with value \"{value?.ToString()}\"");
            this.setValueHandler(component, value);
        }

        protected virtual GetValueHandler CreateGetValueHandler(PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod();
            DynamicMethod getMethod = new DynamicMethod("GetValue", typeof(object), new Type[] { typeof(object) }, typeof(Accessor), true);
            ILGenerator ilGenerator = getMethod.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, getMethodInfo);

            Type returnType = getMethodInfo.ReturnType;

            if (returnType.IsValueType)
                ilGenerator.Emit(OpCodes.Box, returnType);

            ilGenerator.Emit(OpCodes.Ret);

            return getMethod.CreateDelegate(typeof(GetValueHandler)) as GetValueHandler;

        }

        protected virtual SetValueHandler CreateSetValueHandler(PropertyInfo propertyInfo)
        {
            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(false);
            DynamicMethod setPropertyValue = new DynamicMethod("SetValue", typeof(void), new Type[] { typeof(object), typeof(object) }, typeof(Accessor), true);
            ILGenerator ilGenerator = setPropertyValue.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);

            Type parameterType = setMethodInfo.GetParameters()[0].ParameterType;

            if (parameterType.IsValueType)
                ilGenerator.Emit(OpCodes.Unbox_Any, parameterType);

            ilGenerator.Emit(OpCodes.Call, setMethodInfo);
            ilGenerator.Emit(OpCodes.Ret);

            return setPropertyValue.CreateDelegate(typeof(SetValueHandler)) as SetValueHandler;
        }
    }
}
