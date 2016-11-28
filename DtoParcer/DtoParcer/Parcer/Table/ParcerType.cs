using System;

namespace DtoParcer.Parcer.Table
{
    public class ParcerType: IEquatable<ParcerType>
    {
        private readonly string _type;
        private readonly string _format;
        private int _hashCode;

        public ParcerType(string type, string format)
        {
            _type = type;
            _format = format;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = _hashCode;
                if (result != 0) return result;
                result = 17;
                result = 31 * result + _type.GetHashCode();
                result = 31 * result + _format.GetHashCode();
                _hashCode = result;
                return result;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            return obj.GetType() == GetType() && Equals(obj as ParcerType);
        }

        public bool Equals(ParcerType other)
        {
            if (other == null) return false;
            return string.Equals(_type, other._type) && string.Equals(_format, other._format);
        }
    }
}
