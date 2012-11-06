using System.Collections.Generic;
using WindowsInput;

namespace WpfKb.LogicalKeys
{
	/// <summary>
	/// 
	/// </summary>
	public class CtrlAltShiftSensitiveKey : ShiftSensitiveKey
	{
		public CtrlAltShiftSensitiveKey(VirtualKeyCode keyCode, IList<string> keyDisplays)
			: base(keyCode, keyDisplays)
		{
		}
	}
}
