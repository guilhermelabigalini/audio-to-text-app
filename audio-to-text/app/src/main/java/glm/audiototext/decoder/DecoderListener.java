package glm.audiototext.decoder;

import java.io.File;

public interface DecoderListener {
    void onPartialResult(PartialResult message);

    void onFinalResult(FinalResult message);

    void onCompleted();

    void onError(Exception e);
}
