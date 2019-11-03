using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbxAddon.Core
{
    public class PeripheralHooks
    {

        #region Singleton

        private static PeripheralHooks _instance;
        public static PeripheralHooks Instance
        {
            get    
            {
                if (_instance == null)
                {
                    _instance = new PeripheralHooks();
                }
                return _instance;
            }
        }
        private PeripheralHooks()
        {
            Activate();
        }

        #endregion

        public EventHandler<KeyEventArgs> OnKeyUp;
        public EventHandler<KeyEventArgs> OnKeyDown;

        public EventHandler<MouseEventArgs> OnMouseUp;
        public EventHandler<MouseEventExtArgs> OnMouseDown;
        
        // Subroutine for activating the hook
        private void Activate()
        {
            ActivateMouseHook();
            ActivateKeyboardHook();            
        }

        private void ActivateKeyboardHook()
        {
            var m_keyboardListener = new KeyboardHookListener(new GlobalHooker());
            m_keyboardListener.Enabled = true;
            m_keyboardListener.KeyDown += (sender, arg) => OnKeyDown.Invoke(sender, arg);
            m_keyboardListener.KeyUp += (sender, arg) => OnKeyUp.Invoke(sender, arg);
        }

        private void ActivateMouseHook()
        {
            // Note: for an application hook, use the AppHooker class instead
            var m_mouseListener = new MouseHookListener(new GlobalHooker());

            // The listener is not enabled by default
            m_mouseListener.Enabled = true;

            // Set the event handler
            // recommended to use the Extended handlers, which allow input suppression among other additional information
            m_mouseListener.MouseDownExt += (sender,arg) => OnMouseDown.Invoke(sender, arg);
            m_mouseListener.MouseUp += (sender, arg) => OnMouseUp.Invoke(sender, arg);
        }

    }
}
