using System;

namespace LIB.BusinessObjects
{
    /// <summary>
    /// The Permission Enumerator.
    /// </summary>
    [Flags]
    public enum BasePermissionenum : long
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The andministrator area access.
        /// </summary>
        CPAccess = 1,

        /// <summary>
        /// The super admin.
        /// </summary>
        SuperAdmin = 2,

        SMIAccess = 4

    }
}