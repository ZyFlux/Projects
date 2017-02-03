namespace Step
{
    [System.Serializable]
    public class CreateActor : Step
    {
        public enum actorType { Cube, Sphere, Cylinder };
        public string actorId;
        public actorType typeOfActor;
    }
}
