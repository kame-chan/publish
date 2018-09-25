using System;

namespace hyperJLO
{
    public class Joycon
    {
        public bool isLeft;
        public enum state_ : uint
        {
            NOT_ATTACHED = 0,
            DROPPED = 1,
            NO_JOYCONS = 2,
            ATTACHED = 3,
            INPUT_MODE_0x30 = 4,
            IMU_DATA_OK = 5,
        };
        public state_ state;
        private IntPtr handle;
        byte[] default_buf = { 0x0, 0x1, 0x40, 0x40, 0x0, 0x1, 0x40, 0x40 };
        private byte global_count = 0;

        public Joycon(IntPtr handle_, bool left)
        {
            handle = handle_;
            isLeft = left;
        }

        public int Attach(byte leds_ = 0x0)
        {
            state = state_.ATTACHED;
            byte[] a = { 0x0 };
            a[0] = leds_;
            Subcommand(0x30, a, 1);
            Subcommand(0x40, new byte[] { (byte)0x1 }, 1, true);
            return 0;
        }

        public void Detach()
        {
            if (state > state_.NO_JOYCONS)
            {
                Subcommand(0x30, new byte[] { 0x0 }, 1);
                Subcommand(0x40, new byte[] { 0x0 }, 1);
                Subcommand(0x48, new byte[] { 0x0 }, 1);
                Subcommand(0x3, new byte[] { 0x3f }, 1);
            }
            if (state > state_.DROPPED)
            {
                HIDapi.hid_close(handle);
            }
            state = state_.NOT_ATTACHED;
        }

        private byte[] Subcommand(byte sc, byte[] buf, uint len, bool print = true)
        {
            byte[] buf_ = new byte[49];
            byte[] response = new byte[49];
            Array.Copy(default_buf, 0, buf_, 2, 8);
            Array.Copy(buf, 0, buf_, 11, len);
            buf_[10] = sc;
            buf_[1] = global_count;
            buf_[0] = 0x1;
            if (global_count == 0xf) global_count = 0;
            else ++global_count;
            HIDapi.hid_write(handle, buf_, new UIntPtr(len + 11));

            return response;
        }

    }
}