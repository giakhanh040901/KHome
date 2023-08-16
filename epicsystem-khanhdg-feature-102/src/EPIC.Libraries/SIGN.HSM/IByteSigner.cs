namespace EPIC.SIGN.PDF
{
    public interface IByteSigner
    {
        byte[] Sign(byte[] input);
    }
}