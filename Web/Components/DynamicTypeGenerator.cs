using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace mojoPortal.Web.Components;

public class DynamicTypeGenerator
{
	#region Fields

	private readonly AssemblyName _assemblyName = null;
	private TypeBuilder _typeBuilder = null;
	private ILGenerator _ctorIL = null;

	#endregion


	#region Constructor

	private DynamicTypeGenerator(string className)
	{
		_assemblyName = new AssemblyName($"Dynamic_{className}_{Guid.NewGuid()}");

		CreateClass(className);
		CreateConstructor();
	}

	#endregion


	public static DynamicTypeGenerator Init(string className) => new(className);


	#region Public Methods

	public DynamicTypeGenerator AddProperty(string name, Type type)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}

		if (type == null)
		{
			throw new ArgumentNullException("type");
		}

		CreateProperty(name, type);

		return this;
	}


	public DynamicTypeGenerator AddProperties(Dictionary<string, Type> properties)
	{
		foreach (var item in properties)
		{
			AddProperty(item.Key, item.Value);
		}

		return this;
	}


	public Type CreateType()
	{
		// Return from constructor
		//_ctorIL.Emit(OpCodes.Ret);
		return _typeBuilder.CreateType();
	}

	#endregion


	#region  Private Methods

	private void CreateClass(string name)
	{
		var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.Run);
		var moduleBuilder = assemblyBuilder.DefineDynamicModule(_assemblyName.Name);

		_typeBuilder = moduleBuilder.DefineType(
			name,
			TypeAttributes.Public |
			TypeAttributes.Class |
			TypeAttributes.AutoClass |
			TypeAttributes.AnsiClass |
			TypeAttributes.BeforeFieldInit |
			TypeAttributes.AutoLayout,
			null
		);
	}


	private void CreateConstructor()
	{
		_typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
	}


	private void CreateProperty(string propertyName, Type type)
	{
		var pascalName = ToUpperFirstChar(propertyName);

		// Create private field
		var fieldBuilder = _typeBuilder.DefineField($"_{propertyName}", type, FieldAttributes.Private);

		// Create public property
		var propertyBuilder = _typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, type, null);

		// The property "set" and property "get" methods require a special
		// set of attributes.
		var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

		#region Define Getter

		var getPropertyMethodBuilder = _typeBuilder.DefineMethod(
			"get_" + pascalName,
			getSetAttr,
			type,
			Type.EmptyTypes
		);

		var getIl = getPropertyMethodBuilder.GetILGenerator();

		getIl.Emit(OpCodes.Ldarg_0);             // Load "this" onto the evaluation stack
		getIl.Emit(OpCodes.Ldfld, fieldBuilder); // Load the field onto the stack
		getIl.Emit(OpCodes.Ret);                 // Return the value

		#endregion

		#region Define Setter

		var setPropertyMethodBuilder = _typeBuilder.DefineMethod(
			"set_" + pascalName,
			getSetAttr,
			null,
			[type]
		);

		var setIl = setPropertyMethodBuilder.GetILGenerator();

		var modifyProperty = setIl.DefineLabel();
		var exitSet = setIl.DefineLabel();

		setIl.MarkLabel(modifyProperty);
		setIl.Emit(OpCodes.Ldarg_0);             // Load "this" onto the stack
		setIl.Emit(OpCodes.Ldarg_1);             // Load the value argument onto the stack
		setIl.Emit(OpCodes.Stfld, fieldBuilder); // Store the value in the field
		setIl.Emit(OpCodes.Nop);
		setIl.MarkLabel(exitSet);
		setIl.Emit(OpCodes.Ret);                 // Return

		#endregion

		propertyBuilder.SetGetMethod(getPropertyMethodBuilder);
		propertyBuilder.SetSetMethod(setPropertyMethodBuilder);
	}


	private static string ToUpperFirstChar(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}

		return char.ToUpper(input[0]) + input.Substring(1);
	}

	#endregion
}
