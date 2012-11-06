using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;

namespace WpfKb.LogicalKeys
{
	public class CtrlAltShiftAndCaseSensitiveKey : CtrlAltShiftSensitiveKey
	{
		public CtrlAltShiftAndCaseSensitiveKey(VirtualKeyCode keyCode, IList<string> keyDisplays) : base(keyCode, keyDisplays)
		{
		}
	}
}
