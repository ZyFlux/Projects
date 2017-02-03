package step;

import org.json.JSONObject;

public class SendMessage extends Step {
    String senderActorId;
    String receiverActorId;
    String msg;

    public SendMessage(String senderActorId, String receiverActorId, String msg) {
        super(StepType.SEND_MESSAGE);
        this.senderActorId = senderActorId;
        this.receiverActorId = receiverActorId;
        this.msg = msg;
    }

    public SendMessage(JSONObject jsonEvent) {
        super(StepType.SEND_MESSAGE);
        this.senderActorId = jsonEvent.optString("senderActorId", "");
        this.receiverActorId = jsonEvent.optString("receiverActorId", "");
        this.msg = jsonEvent.optString("msg", "");
    }

    public JSONObject toJson() throws Exception {
        JSONObject json = super.toJson();
        json.put("senderActorId", senderActorId).put("receiverActorId", receiverActorId).put("msg", msg);
        return json;
    }
}
