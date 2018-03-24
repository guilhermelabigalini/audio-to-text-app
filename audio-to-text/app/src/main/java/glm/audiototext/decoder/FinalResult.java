package glm.audiototext.decoder;

import java.util.List;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;

public class FinalResult {
    @JsonProperty("RecognitionStatus")
    private long recognitionStatus;
    @JsonProperty("Phrases")
    private List<PartialResult> phrases = null;

    @JsonProperty("RecognitionStatus")
    public long getRecognitionStatus() {
        return recognitionStatus;
    }

    @JsonProperty("RecognitionStatus")
    public void setRecognitionStatus(long recognitionStatus) {
        this.recognitionStatus = recognitionStatus;
    }

    @JsonProperty("Phrases")
    public List<PartialResult> getPhrases() {
        return phrases;
    }

    @JsonProperty("Phrases")
    public void setPhrases(List<PartialResult> phrases) {
        this.phrases = phrases;
    }
}
