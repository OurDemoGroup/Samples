using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using WindowsInput;
using WpfKb.LogicalKeys;

namespace WpfKb.Controls
{
    public class OnScreenKeyboard : Grid
    {

        public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(OnScreenKeyboard), new UIPropertyMetadata(true, OnAreAnimationsEnabledPropertyChanged));

        private ObservableCollection<OnScreenKeyboardSection> _sections;
        private List<ModifierKeyBase> _modifierKeys;
        private List<ILogicalKey> _allLogicalKeys;
        private List<OnScreenKey> _allOnScreenKeys;

        public bool AreAnimationsEnabled
        {
            get { return (bool)GetValue(AreAnimationsEnabledProperty); }
            set { SetValue(AreAnimationsEnabledProperty, value); }
        }

        private static void OnAreAnimationsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var keyboard = (OnScreenKeyboard)d;
            keyboard._allOnScreenKeys.ToList().ForEach(x => x.AreAnimationsEnabled = (bool)e.NewValue);
        }


        public override void BeginInit()
        {
            SetValue(FocusManager.IsFocusScopeProperty, true);
            _modifierKeys = new List<ModifierKeyBase>();
            _allLogicalKeys = new List<ILogicalKey>();
            _allOnScreenKeys = new List<OnScreenKey>();

            _sections = new ObservableCollection<OnScreenKeyboardSection>();
			
            var mainSection = new OnScreenKeyboardSection();

			#region // Germane Keyboard. See: http://msdn.microsoft.com/en-us/library/ms892159.aspx
			var mainKeys = new ObservableCollection<OnScreenKey>()
			{
				new OnScreenKey { GridRow = 0, GridColumn = 0, Key =  new CaseSensitiveKey(VirtualKeyCode.OEM_3, new List<string> { "ö", "Ö" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 1, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_1, new List<string> { "1", "!" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 2, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.VK_2, new List<string> { "2",  "\"" ,"²"})},
			    new OnScreenKey { GridRow = 0, GridColumn = 3, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.VK_3, new List<string> {"3", "§","³"})},
			    new OnScreenKey { GridRow = 0, GridColumn = 4, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_4, new List<string> { "4", "$" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 5, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_5, new List<string> { "5", "%" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 6, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_6, new List<string> { "6", "&" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 7, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.VK_7, new List<string> { "7", "/", "{"})},
			    new OnScreenKey { GridRow = 0, GridColumn = 8, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.VK_8, new List<string> { "8", "(" ,"["})},
			    new OnScreenKey { GridRow = 0, GridColumn = 9, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.VK_9, new List<string> { "9", ")" ,"]"})},
			    new OnScreenKey { GridRow = 0, GridColumn = 10, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.VK_0, new List<string> { "0", "=", "}" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 11, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_MINUS, new List<string> { "-", "_" })},
			    new OnScreenKey { GridRow = 0, GridColumn = 12, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.OEM_PLUS, new List<string> { "+", "*","~"})},
			    new OnScreenKey { GridRow = 0, GridColumn = 13, Key =  new VirtualKey(VirtualKeyCode.BACK, "Bksp"), GridWidth = new GridLength(2, GridUnitType.Star)},

			    new OnScreenKey { GridRow = 1, GridColumn = 0, Key =  new VirtualKey(VirtualKeyCode.TAB, "Tab"), GridWidth = new GridLength(1.5, GridUnitType.Star)},
			    new OnScreenKey { GridRow = 1, GridColumn = 1, Key =  new CtrlAltShiftAndCaseSensitiveKey(VirtualKeyCode.VK_Q, new List<string> { "q", "Q", "@" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 2, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_W, new List<string> { "w", "W" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 3, Key =  new CtrlAltShiftAndCaseSensitiveKey(VirtualKeyCode.VK_E, new List<string> { "e", "E","€" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 4, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_R, new List<string> { "r", "R" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 5, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_T, new List<string> { "t", "T" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 6, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_Y, new List<string> { "y", "Y" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 7, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_U, new List<string> { "u", "U" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 8, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_I, new List<string> { "i", "I" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 9, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_O, new List<string> { "o", "O" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 10, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_P, new List<string> { "p", "P" })},
			    new OnScreenKey { GridRow = 1, GridColumn = 11, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.OEM_4, new List<string> { "ß","?", "\\" })},
				 new OnScreenKey { GridRow = 1, GridColumn = 12, Key =  new CtrlAltShiftSensitiveKey(VirtualKeyCode.OEM_102, new List<string> { "<", ">","|" })},
			    
			    new OnScreenKey { GridRow = 2, GridColumn = 0, Key =  new TogglingModifierKey("Caps", VirtualKeyCode.CAPITAL), GridWidth = new GridLength(1.7, GridUnitType.Star)},
			    new OnScreenKey { GridRow = 2, GridColumn = 1, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_A, new List<string> { "a", "A" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 2, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_S, new List<string> { "s", "S" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 3, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_D, new List<string> { "d", "D" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 4, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_F, new List<string> { "f", "F" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 5, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_G, new List<string> { "g", "G" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 6, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_H, new List<string> { "h", "H" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 7, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_J, new List<string> { "j", "J" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 8, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_K, new List<string> { "k", "K" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 9, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_L, new List<string> { "l", "L" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 10, Key =  new CaseSensitiveKey(VirtualKeyCode.OEM_1, new List<string> { "ü", "Ü" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 11, Key =  new CaseSensitiveKey(VirtualKeyCode.OEM_7, new List<string> { "ä", "Ä" })},
			    new OnScreenKey { GridRow = 2, GridColumn = 12, Key =  new VirtualKey(VirtualKeyCode.RETURN, "Enter"), GridWidth = new GridLength(1.8, GridUnitType.Star)},

			    new OnScreenKey { GridRow = 3, GridColumn = 0, Key =  new InstantaneousModifierKey("Shift", VirtualKeyCode.SHIFT), GridWidth = new GridLength(2.4, GridUnitType.Star)},
				new OnScreenKey { GridRow = 3, GridColumn = 1, Key =  new InstantaneousModifierKey("Ctrl", VirtualKeyCode.CONTROL), GridWidth = new GridLength(2.4, GridUnitType.Star)},
			    new OnScreenKey { GridRow = 3, GridColumn = 2, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_Z, new List<string> { "z", "Z" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 3, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_X, new List<string> { "x", "X" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 4, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_C, new List<string> { "c", "C" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 5, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_V, new List<string> { "v", "V" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 6, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_B, new List<string> { "b", "B" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 7, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_N, new List<string> { "n", "N" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 8, Key =  new CtrlAltShiftAndCaseSensitiveKey(VirtualKeyCode.VK_M, new List<string> { "m", "M", "µ" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 9, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_COMMA, new List<string> { ",", ";" })},
			    new OnScreenKey { GridRow = 3, GridColumn = 10, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_PERIOD, new List<string> { ".", ":" })},
				new OnScreenKey { GridRow = 0, GridColumn = 11, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_2, new List<string> {"#", "`", })},
				new OnScreenKey { GridRow = 3, GridColumn = 12, Key =  new InstantaneousModifierKey("Ctrl", VirtualKeyCode.CONTROL), GridWidth = new GridLength(2.4, GridUnitType.Star)},
			    new OnScreenKey { GridRow = 3, GridColumn = 13, Key =  new InstantaneousModifierKey("Shift", VirtualKeyCode.SHIFT), GridWidth = new GridLength(2.4, GridUnitType.Star)},

				new OnScreenKey { GridRow = 4, GridColumn = 0, Key =  new VirtualKey(VirtualKeyCode.SPACE, " "), GridWidth = new GridLength(5, GridUnitType.Star)},
			};
			#endregion

			#region // United States 101 Keyboard. See : http://msdn.microsoft.com/en-us/library/ms894073.aspx
			//{
		//    new OnScreenKey { GridRow = 0, GridColumn = 0, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_3, new List<string> { "`", "~" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 1, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_1, new List<string> { "1", "!" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 2, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_2, new List<string> { "2", "@" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 3, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_3, new List<string> { "3", "#" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 4, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_4, new List<string> { "4", "$" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 5, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_5, new List<string> { "5", "%" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 6, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_6, new List<string> { "6", "^" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 7, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_7, new List<string> { "7", "&" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 8, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_8, new List<string> { "8", "*" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 9, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_9, new List<string> { "9", "(" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 10, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_0, new List<string> { "0", ")" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 11, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_MINUS, new List<string> { "-", "_" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 12, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_PLUS, new List<string> { "=", "+" })},
		//    new OnScreenKey { GridRow = 0, GridColumn = 13, Key =  new VirtualKey(VirtualKeyCode.BACK, "Bksp"), GridWidth = new GridLength(2, GridUnitType.Star)},

		//    new OnScreenKey { GridRow = 1, GridColumn = 0, Key =  new VirtualKey(VirtualKeyCode.TAB, "Tab"), GridWidth = new GridLength(1.5, GridUnitType.Star)},
		//    new OnScreenKey { GridRow = 1, GridColumn = 1, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_Q, new List<string> { "q", "Q" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 2, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_W, new List<string> { "w", "W" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 3, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_E, new List<string> { "e", "E" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 4, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_R, new List<string> { "r", "R" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 5, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_T, new List<string> { "t", "T" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 6, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_Y, new List<string> { "y", "Y" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 7, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_U, new List<string> { "u", "U" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 8, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_I, new List<string> { "i", "I" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 9, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_O, new List<string> { "o", "O" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 10, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_P, new List<string> { "p", "P" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 11, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_4, new List<string> { "[", "{" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 12, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_6, new List<string> { "]", "}" })},
		//    new OnScreenKey { GridRow = 1, GridColumn = 13, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_5, new List<string> { "\\", "|" }), GridWidth = new GridLength(1.3, GridUnitType.Star)},

		//    new OnScreenKey { GridRow = 2, GridColumn = 0, Key =  new TogglingModifierKey("Caps", VirtualKeyCode.CAPITAL), GridWidth = new GridLength(1.7, GridUnitType.Star)},
		//    new OnScreenKey { GridRow = 2, GridColumn = 1, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_A, new List<string> { "a", "A" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 2, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_S, new List<string> { "s", "S" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 3, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_D, new List<string> { "d", "D" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 4, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_F, new List<string> { "f", "F" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 5, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_G, new List<string> { "g", "G" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 6, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_H, new List<string> { "h", "H" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 7, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_J, new List<string> { "j", "J" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 8, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_K, new List<string> { "k", "K" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 9, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_L, new List<string> { "l", "L" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 10, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_1, new List<string> { ";", ":" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 11, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_7, new List<string> { "\"", "\"" })},
		//    new OnScreenKey { GridRow = 2, GridColumn = 12, Key =  new VirtualKey(VirtualKeyCode.RETURN, "Enter"), GridWidth = new GridLength(1.8, GridUnitType.Star)},

		//    new OnScreenKey { GridRow = 3, GridColumn = 0, Key =  new InstantaneousModifierKey("Shift", VirtualKeyCode.SHIFT), GridWidth = new GridLength(2.4, GridUnitType.Star)},
		//    new OnScreenKey { GridRow = 3, GridColumn = 1, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_Z, new List<string> { "z", "Z" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 2, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_X, new List<string> { "x", "X" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 3, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_C, new List<string> { "c", "C" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 4, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_V, new List<string> { "v", "V" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 5, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_B, new List<string> { "b", "B" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 6, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_N, new List<string> { "n", "N" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 7, Key =  new CaseSensitiveKey(VirtualKeyCode.VK_M, new List<string> { "m", "M" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 8, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_COMMA, new List<string> { ",", "<" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 9, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_PERIOD, new List<string> { ".", ">" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 10, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_2, new List<string> { "/", "?" })},
		//    new OnScreenKey { GridRow = 3, GridColumn = 11, Key =  new InstantaneousModifierKey("Shift", VirtualKeyCode.SHIFT), GridWidth = new GridLength(2.4, GridUnitType.Star)},

		//    new OnScreenKey { GridRow = 4, GridColumn = 0, Key =  new VirtualKey(VirtualKeyCode.SPACE, " "), GridWidth = new GridLength(5, GridUnitType.Star)},
			//};
#endregion

			mainSection.Keys = mainKeys;
            mainSection.SetValue(ColumnProperty, 0);
            _sections.Add(mainSection);
            ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(3, GridUnitType.Star)});
            Children.Add(mainSection);

            _allLogicalKeys.AddRange(mainKeys.Select(x => x.Key));
            _allOnScreenKeys.AddRange(mainSection.Keys);

            _modifierKeys.AddRange(_allLogicalKeys.OfType<ModifierKeyBase>());
            _allOnScreenKeys.ForEach(x => x.OnScreenKeyPress += OnScreenKeyPress);

            SynchroniseModifierKeyState();

            base.BeginInit();
        }

        private void OnScreenKeyPress(DependencyObject sender, OnScreenKeyEventArgs e)
        {
            if (e.OnScreenKey.Key is ModifierKeyBase)
            {
                var modifierKey = (ModifierKeyBase)e.OnScreenKey.Key;
				switch (modifierKey.KeyCode)
	            {
					case VirtualKeyCode.SHIFT:
						HandleShiftKeyPressed(modifierKey);
						break;
					case VirtualKeyCode.CONTROL:
						HandleControlKeyPressed(modifierKey);
			            break;
					case VirtualKeyCode.CAPITAL:
						HandleCapsLockKeyPressed(modifierKey);
			            break;
					case VirtualKeyCode.NUMLOCK:
						HandleNumLockKeyPressed(modifierKey);
			            break;
	            }
            }
            else
            {
                ResetInstantaneousModifierKeys();
            }

            _modifierKeys.OfType<InstantaneousModifierKey>().ToList().ForEach(x => x.SynchroniseKeyState());
        }

        private void SynchroniseModifierKeyState()
        {
            _modifierKeys.ToList().ForEach(x => x.SynchroniseKeyState());
        }

        private void ResetInstantaneousModifierKeys()
        {
            _modifierKeys.OfType<InstantaneousModifierKey>().ToList().ForEach(x => { if (x.IsInEffect) x.Press(); });
        }

		private void HandleControlKeyPressed(ModifierKeyBase controlKey)
		{
			bool isShiftdown = InputSimulator.IsKeyDown(VirtualKeyCode.SHIFT);
			int selectedIndex = 0;
			if (isShiftdown)
				selectedIndex = controlKey.IsInEffect ? 2 : 1;

			_allLogicalKeys.OfType<CtrlAltShiftSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = selectedIndex);
		}

        private void HandleShiftKeyPressed(ModifierKeyBase shiftKey)
        {
	        int selectedIndex = InputSimulator.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL) ^ shiftKey.IsInEffect ? 1 : 0;
	        
			_allLogicalKeys.OfType<CaseSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = selectedIndex);
            _allLogicalKeys.OfType<ShiftSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = shiftKey.IsInEffect ? 1 : 0);
        }

        private void HandleCapsLockKeyPressed(ModifierKeyBase capsLockKey)
        {
	        int selectedIndex = capsLockKey.IsInEffect ^ InputSimulator.IsKeyDownAsync(VirtualKeyCode.SHIFT) ? 1 : 0;
			_allLogicalKeys.OfType<CaseSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = selectedIndex);
			_allLogicalKeys.OfType<CtrlAltShiftAndCaseSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = selectedIndex);
        }

        private void HandleNumLockKeyPressed(ModifierKeyBase numLockKey)
        {
            _allLogicalKeys.OfType<NumLockSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = numLockKey.IsInEffect? 1 : 0);
        }
    }
}