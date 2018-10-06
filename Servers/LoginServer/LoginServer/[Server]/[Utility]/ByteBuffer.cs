using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPS.LoginServer.Utility
{
    public class ByteBuffer
    {
        private List<byte> _writeBuffer;
        private byte[] _readBuffer;
        private int _readPostition;
        private bool _bufferUpdate = false;


        public ByteBuffer()
        {
            _writeBuffer = new List<byte>();
            _readPostition = 0;
        }

        public int GetReadPosition()
        {
            return _readPostition;
        }

        public byte[] ToArray()
        {
            return _writeBuffer.ToArray();
        }

        public int Count()
        {
            return _writeBuffer.Count;
        }

        public int Length()
        {
            return Count() - _readPostition;
        }

        public void Clear()
        {
            _writeBuffer.Clear();
            _readPostition = 0;
        }

        #region Write Data

        public void WriteByte(byte Input)
        {
            _writeBuffer.Add(Input);
            _bufferUpdate = true;
        }

        public void WriteBytes(byte[] Input)
        {
            _writeBuffer.AddRange(Input);
            _bufferUpdate = true;
        }

        public void WriteShort(short Input)
        {
            _writeBuffer.AddRange(BitConverter.GetBytes(Input));
            _bufferUpdate = true;
        }

        public void WriteInt(int Input)
        {
            _writeBuffer.AddRange(BitConverter.GetBytes(Input));
            _bufferUpdate = true;
        }

        public void WriteFloat(float Input)
        {
            _writeBuffer.AddRange(BitConverter.GetBytes((double)Input));
            _bufferUpdate = true;
        }

        public void WriteString(string Input)
        {
            _writeBuffer.AddRange(BitConverter.GetBytes(Input.Length));
            _writeBuffer.AddRange(Encoding.ASCII.GetBytes(Input));
            _bufferUpdate = true;
        }

        public void WriteBool(bool Input)
        {
            WriteShort(Input ? (short)1 : (short)0);
            _bufferUpdate = true;
        }

        #endregion

        #region Read Data
        public byte ReadByte(bool Peek = true)
        {
            if (_bufferUpdate)
            {
                _readBuffer = _writeBuffer.ToArray();
                _bufferUpdate = false;
            }

            _readPostition += 1;
            return _readBuffer[_readPostition - 1];
        }

        public byte[] ReadBytes(int Lenght, bool Peek = true)
        {
            if (_bufferUpdate)
            {
                _readBuffer = _writeBuffer.ToArray();
                _bufferUpdate = false;
            }

            byte[] result = new byte[Lenght];
            if (Lenght > 0)
            {
                for (int i = 0; i < Lenght; i++)
                {
                    result[i] = _readBuffer[_readPostition + i];
                }

                _readPostition += Lenght;
            }

            return result;
        }

        public short ReadShort(bool Peek = true)
        {
            if (_bufferUpdate)
            {
                _readBuffer = _writeBuffer.ToArray();
                _bufferUpdate = false;
            }

            short result = BitConverter.ToInt16(_readBuffer, _readPostition);
            _readPostition += 2;

            return result;
        }

        public int ReadInt(bool Peek = true)
        {
            if (_bufferUpdate)
            {
                _readBuffer = _writeBuffer.ToArray();
                _bufferUpdate = false;
            }

            int result = BitConverter.ToInt32(_readBuffer, _readPostition);
            _readPostition += 4;

            return result;
        }

        public float ReadFloat(bool Peek = true)
        {
            if (_bufferUpdate)
            {
                _readBuffer = _writeBuffer.ToArray();
                _bufferUpdate = false;
            }

            float result = (float)BitConverter.ToDouble(_readBuffer, _readPostition);
            _readPostition += 8;

            return result;
        }

        public string ReadString(bool Peek = true)
        {
            int stringLength = ReadInt(true);
            if (_bufferUpdate)
            {
                _readBuffer = _writeBuffer.ToArray();
                _bufferUpdate = false;
            }

            string result = Encoding.ASCII.GetString(_readBuffer, _readPostition, stringLength);
            if (result.Length > 0)
            {
                _readPostition += stringLength;
            }

            return result;
        }

        public bool ReadBool(bool Peek = true)
        {
            short result = ReadShort(true);
            return (result == 1);
        }

        #endregion
    }
}
