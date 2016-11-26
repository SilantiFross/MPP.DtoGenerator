using System;

namespace DtoParcer.Parcer.Table
{
    public class ParcerType: IEquatable<ParcerType>
    {
        public string Type;
        public string Format;
        private int _hashCode;

        public ParcerType(string type, string format)
        {
            Type = type;
            Format = format;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = _hashCode;
                if (result != 0) return result;
                result = 17;
                result = 31 * result + Type.GetHashCode();
                result = 31 * result + Format.GetHashCode();
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
            return string.Equals(Type, other.Type) && string.Equals(Format, other.Format);
        }
    }
}
