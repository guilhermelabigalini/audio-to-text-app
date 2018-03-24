package glm.audiototext.decoder;

import java.io.File;

public interface DecoderListener {
    void onPartialResult(String message);

    void onFailed(String message);
}
