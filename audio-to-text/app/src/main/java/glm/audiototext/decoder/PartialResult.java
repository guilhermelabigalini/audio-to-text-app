package glm.audiototext.decoder;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;

public class PartialResult {

    @JsonProperty("DisplayText")
    private String displayText;
    @JsonProperty("LexicalForm")
    private String lexicalForm;
    @JsonProperty("Confidence")
    private long confidence;
    @JsonProperty("MediaTime")
    private long mediaTime;
    @JsonProperty("MediaDuration")
    private long mediaDuration;

    @JsonProperty("DisplayText")
    public String getDisplayText() {
        return displayText;
    }

    @JsonProperty("DisplayText")
    public void setDisplayText(String displayText) {
        this.displayText = displayText;
    }

    @JsonProperty("LexicalForm")
    public String getLexicalForm() {
        return lexicalForm;
    }

    @JsonProperty("LexicalForm")
    public void setLexicalForm(String lexicalForm) {
        this.lexicalForm = lexicalForm;
    }

    @JsonProperty("Confidence")
    public long getConfidence() {
        return confidence;
    }

    @JsonProperty("Confidence")
    public void setConfidence(long confidence) {
        this.confidence = confidence;
    }

    @JsonProperty("MediaTime")
    public long getMediaTime() {
        return mediaTime;
    }

    @JsonProperty("MediaTime")
    public void setMediaTime(long mediaTime) {
        this.mediaTime = mediaTime;
    }

    @JsonProperty("MediaDuration")
    public long getMediaDuration() {
        return mediaDuration;
    }

    @JsonProperty("MediaDuration")
    public void setMediaDuration(long mediaDuration) {
        this.mediaDuration = mediaDuration;
    }

}