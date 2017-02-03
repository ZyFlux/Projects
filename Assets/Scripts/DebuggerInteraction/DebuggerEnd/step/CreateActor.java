package step;

import org.json.JSONObject;

public class CreateActor extends Step {
    String actorId;

    public CreateActor(String actorId) {
        super(StepType.CREATE_ACTOR);
        this.actorId = actorId;
    }

    public CreateActor(JSONObject jsonEvent) {
        super(StepType.CREATE_ACTOR);
        this.actorId = jsonEvent.optString("actorId", "");
    }

    public JSONObject toJson() throws Exception {
        JSONObject json = super.toJson();
        json.put("actorId", actorId);
        return json;
    }
}
