namespace Step
{
    [System.Serializable]
    public class SendMessage : Step
    {
        public string senderActorId;
        public string receiverActorId;
        public string msg;
    }
}
