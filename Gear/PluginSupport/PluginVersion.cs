﻿/* --------------------------------------------------------------------------------
 * Gear: Parallax Inc. Propeller Debugger
 * Copyright 2014 - Antonio Sanhueza
 * --------------------------------------------------------------------------------
 * PluginVersion.cs
 * Version attribute class for emulator plugins
 * --------------------------------------------------------------------------------
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * --------------------------------------------------------------------------------
 */

using System;

namespace Gear.PluginSupport
{
    /// @brief Versioning support for members of PluginBase.
    /// This class have the definitions of which methods of `PluginBase` will be versionated, 
    /// e.g. that have different parameters by the same method name and are controlled by a
    /// version number (ex. 0.0, 1.0, 1.1, etc.).
    /// Each time a variant is generated, the developer have to modify this class:
    /// Case 1 - new method to versionate not managed before : add a memberType enumeration 
    /// value for a new method of PluginBase to versionate, and a versionatedMember 
    /// enumeration value for each version added. Then apply the VersionAttribute attribute
    /// to the method with the new memberType enumeration value added and appropriate 
    /// version number (ex 0.0 if is a new method).
    /// Case 2 - new version of a method already managed : add only a new versionatedMember 
    /// enumeration value based on the corresponding memberType enumeration value that exists. 
    /// Then apply the VersionAttribute attribute to the method with the same memberType 
    /// enumeration value and appropriate version number.
    /// @version 14.8.7 - Added.
    public class PluginVersioning
    {
        /// @brief Type of member for version management on menbers of Plugins.
        /// They should resemble the name of the method for mnemonics and easy coding.
        /// <summary>
        /// Type of member for version management on menbers of Plugins.
        /// They should resemble the name of the method for mnemonics and easy coding.
        /// </summary>
        public enum memberType
        {
            none = 0,
            OnClock,        //notifies tick clocks 
            OnPinChange     //notifies pin changes
        }

        /// @brief Versions of members to identify.
        /// When added a new member version, have to register a new value to manage it.
        /// <summary>
        /// Versions of members to identify. When added a new member version, have to register 
        /// a new value to manage it.
        /// </summary>
        public enum versionatedMember
        {
            none = 0,
            OnClockV0_0,    //
            OnClockV1_0,    //
            OnPinChangeV0_0 //
        }

        ///=======================================================================
        ///Declare in this section delegates for each version of members to manage.
        #region Delegates for each versionated member of PluginBase.
        /// @brief Delegate for version 0.0 for OnClock.
        [VersionAttribute(
            0.0f, 
            1.0f, 
            PluginVersioning.memberType.OnClock,
            CodeVersionatedMember = PluginVersioning.versionatedMember.OnClockV0_0)]
        public delegate void TickHandlerV0_0(double time);
        /// @brief Delegate for version 1.0 for OnClock.
        [Version(
            1.0f, 
            PluginVersioning.memberType.OnClock,
            CodeVersionatedMember = PluginVersioning.versionatedMember.OnClockV1_0)]
        public delegate void TickHandlerV1_0(uint sysCounter, double time);
        /// @brief Delegate for version 0.0 for OnClock.
        [VersionAttribute(
            0.0f,
            PluginVersioning.memberType.OnPinChange,
            CodeVersionatedMember = PluginVersioning.versionatedMember.OnPinChangeV0_0)]
        public delegate void OnPinChangeV0_0(double time);
        ///=======================================================================
        #endregion
        

    }

    /// @brief
    public class VersRange
    {
        /// @brief Lower limit to validity.
        private float _verFrom;
        /// @brief Upper limit to validity. Maximun limit could be +infinity.
        private float _verTo;
        /// @brief Include lower value (=true) or excludes (=false).
        private bool _includeLower;
        /// @brief Include upper value (=true) or excludes (=false).
        private bool _includeUpper;

        /// @brief Constructor with lower limit from a value (included).
        /// Assumed upper or equal from this value up to +infinity.
        /// @param versionFrom Lower limit for valid version.
        /// <summary>
        /// Constructor with lower limit from a value (included).
        /// Assumed upper or equal from this value up to +infinity.
        /// </summary>
        /// <param name="versionFrom">Lower limit for valid version.</param>
        public VersRange(float versionFrom)
        {
            _verFrom = versionFrom;
            _verTo = Single.MaxValue;
            _includeLower = true;
            _includeUpper = true;
        }
        /// @brief Constructor for validity between two values.
        /// Assumed upper or equal from lower limit and lesser than upper limit.
        /// @param[in] versionFrom Lower limit for valid version (included).
        /// @param[in] versionTo Upper limit for valid version (not included).
        /// <summary>
        /// Constructor for validity between two values.
        /// Assumed upper or equal from lower limit and lesser than upper limit.
        /// </summary>
        /// <param name="versionFrom">Lower limit for valid version (included).</param>
        /// <param name="versionTo">Upper limit for valid version (not included).</param>
        public VersRange(float versionFrom, float versionTo)
        {
            //TODO [ASB] : throw exception if versionXXXX is out of range, ex. lower than 0.0
            _verFrom = versionFrom;
            _verTo = versionTo;
            _includeLower = true;
            _includeUpper = false;
        }

        /// @brief Getter to include lower limit o not.
        /// <summary>
        /// Getter to include lower limit o not.
        /// </summary>
        public float VersionFrom { get { return _verFrom; } }
        /// @brief Getter to include upper limit o not.
        /// <summary>
        /// Getter to include upper limit o not.
        /// </summary>
        public float VersionTo { get { return _verTo; } }
        /// @brief Property to include or not the lower limit on validity.
        /// <summary>
        /// Property to include or not the lower limit on validity.
        /// </summary>
        public bool IncludeLower
        {
            get { return _includeLower; }
            set { _includeLower = value; }
        }
        /// @brief Property to include or not the upper limit on validity.
        /// <summary>
        /// Property to include or not the upper limit on validity.
        /// </summary>
        public bool IncludeUpper
        {
            get { return _includeUpper; }
            set { _includeUpper = value; }
        }

        /// @brief Validate if atributte is valid beetween lower and upper limits.
        /// @param[in] version Version number to test validity.
        /// @returns True if between limits, false if not.
        /// <summary>
        /// Validate if atributte is valid beetween lower and upper limits.
        /// </summary>
        /// <param name="version">Version number to test validity.</param>
        /// <returns>True if between limits, false if not.</returns>
        public bool ValidOn(float version)
        {
            return (
                (_includeLower ? (_verFrom <= version) : (_verFrom < version)) &
                (_includeUpper ? (version <= _verTo) : (version < _verTo))
            );
        }

    }

    /// @brief Attribute class for manage versioning for members of PluginBase.
    /// To be applied as attribute to members to have dinamic manage of versions.
    /// @version 14.8.5 - Added.
    /// <summary>
    /// Attribute class for manage versioning for members of PluginBase.
    /// To be applied as attribute to members to have dinamic manage of versions.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate, 
        AllowMultiple = false, Inherited = true)]
    public class VersionAttribute : Attribute
    {
        
        /// @brief Range of versioning.
        private VersRange _range;
        /// @brief Type of member to versioning.
        private PluginVersioning.memberType _memberType;
        /// @brief Code of a versionated member.
        private PluginVersioning.versionatedMember _versionatedMember;

        #region Constructor for class VersionAttribute
        /// @brief Constructor with lower limit of validity.
        /// Typically used by a new version of a method (upper range of validity).
        /// Assumed upper or equal from this value up to +infinity.
        /// @param[in] versionFrom Lower limit for valid version.
        /// @param[in] memberType Type of member to versioning.
        /// <summary>
        /// Constructor with lower limit from a value (included).
        /// Assumed upper or equal from this value up to +infinity.
        /// </summary>
        /// <param name="versionFrom">Lower limit for valid version.</param>
        /// <param name="memberType">Type of member to versioning.</param>
        public VersionAttribute(float versionFrom, PluginVersioning.memberType memberType)
        {
            //TODO [ASB] : add support for exceptions trowed by VersRange
            _range = new VersRange(versionFrom);
            _memberType = memberType;
            _versionatedMember = PluginVersioning.versionatedMember.none;
        }

        /// @brief Constructor with both limits for validity.
        /// Implies upper or equal from `lowerLimit` and less than `upperLimit`.
        /// @param[in] versionFrom Lower limit for valid version.
        /// @param[in] upperLimit Upper limit for valid version.
        /// @param[in] memberType Type of member to versioning.
        /// <summary>
        /// Constructor with lower limit from a value (included).
        /// Assumed upper or equal from this value up to +infinity.
        /// </summary>
        /// <param name="versionFrom">Lower limit for valid version.</param>
        /// <param name="upperLimit">Upper limit for valid version.</param>
        /// <param name="memberType">Type of member to versioning.</param>
        public VersionAttribute(float lowerLimit, float upperLimit, PluginVersioning.memberType memberType)
        {
            //TODO [ASB] : add support for exceptions trowed by VersRange
            _range = new VersRange(lowerLimit, upperLimit);
            _memberType = memberType;
            _versionatedMember = PluginVersioning.versionatedMember.none;
        }
        #endregion

        #region Properties of VersionAttribute class
        /// @brief Property for versionated member enumeration.
        /// <summary>
        /// Property for versionated member enumeration.
        /// </summary>
        private PluginVersioning.versionatedMember AssignVersionedMember
        {
            get { return _versionatedMember; }
            set { _versionatedMember = value; }
        }

        /// @brief Property for the code of a versionated member.
        /// <summary>
        /// Property for the code of a versionated member.
        /// </summary>
        public PluginVersioning.versionatedMember CodeVersionatedMember
        {
            get { return _versionatedMember;  }
            set { _versionatedMember = value; }
        }
        #endregion

        /// @brief Validate if atributte is valid beetween lower and upper limits of permitted range.
        /// @param[in] version Version number to test validity.
        /// @returns True if between limits, false if not.
        /// <summary>
        /// Validate if atributte is valid beetween lower and upper limits of permitted range.
        /// </summary>
        /// <param name="version">Version number to test validity.</param>
        /// <returns>True if between limits, false if not.</returns>
        /// TODO [ASB] : define if it wolud be used only inside (=> change to private) or not.
        public bool ValidOnVersion(float version) 
        {
            return _range.ValidOn(version);
        }
    }

    /// @brief Manages Versions of plugins.
    /// Manages versions of plugins, to choose correct member signatures for each version
    /// of plugin system.
    class VersionatedPluginContainer
    {
        /// @brief Pointer to plugin.
        private PluginBase _plugin;
        /// @brief version of plugin system to use
        private float _version;
        /// @brief Type of member to select.
        private PluginVersioning.memberType _memType;

        /// @brief Constructor with member type specification.
        /// By default every VersionatedPluginContainer has version =0.0
        public VersionatedPluginContainer(PluginBase plugin, float Version)
        {
            _plugin = plugin;
            _version = Version;
            _memType = PluginVersioning.memberType.none;
        }

        /// @brief Attribute to hold target version
        public PluginVersioning.memberType memberType
        {
            get { return _memType; }
            set { _memType = value; }
        }

        /// @brief Get member code by type and version.
        /// To be used 
        private PluginVersioning.versionatedMember GetMember(PluginVersioning.memberType member)
        {
            PluginVersioning.versionatedMember vmemb = PluginVersioning.versionatedMember.none;
            //TODO [ASB] : agregar lógica para determinar el tipo de miembro según versión, y ejecutarlo
            switch (member)
            {
                case PluginVersioning.memberType.OnPinChange:

                    break;
                case PluginVersioning.memberType.OnClock:

                    break;
            };

            return vmemb;
        }
    }

}