using System.Collections;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx
/// </summary>

//any object on C# 4.0 has 4 default member method
//1. Equals 2.GetHashCode 3.GetType 4.ToString

public class AttributesTest : MonoBehaviour
{

    // The IsTested class is a user-defined custom attribute class.
    // It can be applied to any declaration including
    //  - types (struct, class, enum, delegate)
    //  - members (methods, fields, events, properties, indexers)
    // It is used with no arguments.
    public class IsTestedAttribute : Attribute
    {
        public override string ToString()
        {
            return "Is Tested";
        }
    }

    // The AuthorAttribute class is a user-defined attribute class.
    // It can be applied to classes and struct declarations only.
    // It takes one unnamed string argument (the author's name).
    // It has one optional named argument Version, which is of type int.
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/tw5zxet9(v=vs.71).aspx                //Info about AttributeUsage
    /// https://msdn.microsoft.com/en-us/library/system.attributetargets(v=vs.71).aspx //Info about AttributeTargets.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class AuthorAttribute : Attribute
    {
        // This constructor specifies the unnamed arguments to the attribute class.
        public AuthorAttribute(string name)
        {
            this.name = name;
            this.version = 0;
        }

        // This property is readonly (it has no set accessor)
        // so it cannot be used as a named argument to this attribute.
        public string Name
        {
            get
            {
                return name;
            }
        }

        // This property is read-write (it has a set accessor)
        // so it can be used as a named argument when using this
        // class as an attribute class.
        public int Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        public override string ToString()
        {
            string value = "Author : " + Name;
            if (version != 0)
            {
                value += " Version : " + Version.ToString();
            }
            return value;
        }

        private string name;
        private int version;
    }

    // Here you attach the AuthorAttribute user-defined custom attribute to 
    // the Account class. The unnamed string argument is passed to the 
    // AuthorAttribute class's constructor when creating the attributes.
    [Author("Joe Programmer")]
    class Account
    {
        // Attach the IsTestedAttribute custom attribute to this method.
        [IsTested]
        public void AddOrder(Order orderToAdd)
        {
            orders.Add(orderToAdd);
        }

        private ArrayList orders = new ArrayList();
    }

    // Attach the AuthorAttribute and IsTestedAttribute custom attributes 
    // to this class.
    // Note the use of the 'Version' named argument to the AuthorAttribute.
    [Author("Jane Programmer", Version = 2), IsTested()]
    class Order
    {
        // add stuff here ...
    }

    public class MainClass
    {
        private static bool IsMemberTested(MemberInfo member)
        {
            foreach (object attribute in member.GetCustomAttributes(true))
            {
                if (attribute is IsTestedAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        private static void DumpAttributes(MemberInfo member)
        {
            Debug.Log("Attributes for : " + member.Name);
            foreach(object attribute in member.GetCustomAttributes(true))
            {
                Debug.Log(attribute);
            }
        }

        public static void MainTest()
        {
            // display attributes for Account class
            DumpAttributes(typeof(Account));

            // display list of tested members
            foreach (MethodInfo method in (typeof(Account)).GetMethods())
            {
                //check if the method has the attribute of IsTestedAttribute
                if (IsMemberTested(method))
                {
                    Debug.LogFormat("Member {0} is tested!", method.Name);
                }
                else
                {
                    Debug.LogFormat("Member {0} is NOT tested!", method.Name);
                }
            }

            // display attributes for Order class
            DumpAttributes(typeof(Order));

            // display attributes for methods on the Order class
            foreach (MethodInfo method in (typeof(Order)).GetMethods())
            {
                if (IsMemberTested(method))
                {
                    Debug.LogFormat("Member {0} is tested!", method.Name);
                }
                else
                {
                    Debug.LogFormat("Member {0} is NOT tested!", method.Name);
                }
            }
        }
    }

}
