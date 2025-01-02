namespace EstateManagementUI.UITests;

public static class TestHelpers {
    public static T GetPropertyValue<T>(object obj, string propertyName)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var type = obj.GetType();
        var property = type.GetProperty(propertyName);

        if (property == null)
            throw new ArgumentException($"Property '{propertyName}' not found on type '{type.FullName}'.");

        var value = property.GetValue(obj);

        if (value == null)
            throw new InvalidOperationException($"Property '{propertyName}' is null.");

        if (value is not T)
            throw new InvalidCastException($"Property '{propertyName}' is not of type '{typeof(T).FullName}'.");

        return (T)value;
    }
}