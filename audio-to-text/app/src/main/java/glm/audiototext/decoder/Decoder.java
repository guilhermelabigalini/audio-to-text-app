package glm.audiototext.decoder;

import android.content.Context;
import android.net.Uri;
import android.util.Log;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class Decoder {
    private static final String TAG = "Decoder";
    private static final String Charset = "UTF-8";
    private static final String DecodeURL = "https://audiototextserviceweb.azurewebsites.net/Decode/Post";

    private final Context context;
    private final ObjectMapper objectMapper;

    public Decoder(Context context) {
        this.context = context;
        this.objectMapper = new ObjectMapper();
    }

    private void notifyEvent(DecoderListener listener, String message) throws IOException {
        if (message.indexOf("RecognitionStatus") > 0) {
            listener.onFinalResult(this.objectMapper.readValue(message, FinalResult.class));
        } else if (message.indexOf("DisplayText") > 0) {
            listener.onPartialResult(this.objectMapper.readValue(message, PartialResult.class));
        }
    }

    public void decode(Uri resource, String culture, DecoderListener listener) throws IOException {
        try (InputStream resourceStream = context.getContentResolver().openInputStream(resource)) {
            try (MultipartUtility multipart = new MultipartUtility(DecodeURL, Charset)) {
                multipart.addFormField("culture", culture);
                multipart.addFilePart("file", "audio", resourceStream);
                //String response = multipart.finish(); // response from server.

                try (BufferedReader reader = new BufferedReader(new InputStreamReader(
                        multipart.getInputStream()))) {
                    String line = null;
                    while ((line = reader.readLine()) != null) {
                        // response.append(line);
                        Log.d(TAG, "get row:" + line);
                        notifyEvent(listener, line);
                    }
                }
            }
        } catch (Exception e) {
            listener.onError(e);
        }  finally {
            listener.onCompleted();
        }
    }
}
