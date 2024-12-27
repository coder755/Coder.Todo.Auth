using System.Reflection;
using EntityFramework.Exceptions.Common;

namespace Coder.Todo.Auth.Test.Services;

public static class ServiceTestUtils
{
    public static UniqueConstraintException GetUniqueConstraintException(string constraintName)
    {
        var uniqueConstraintException = new UniqueConstraintException();
        var t = uniqueConstraintException.GetType();
        const BindingFlags invokeAtt = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance;
        t.InvokeMember("ConstraintName", invokeAtt, null, uniqueConstraintException, [constraintName]);
        return uniqueConstraintException;
    }
}