package glm.audiototext.decoder;

import android.content.Context;
import android.net.Uri;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;

public class Decoder {
    private static final String TAG = "Decoder";
    private static final String Charset = "UTF-8";
    private static final String DecodeURL = "https://audiototextserviceweb.azurewebsites.net/Decode/Post";

    private final Context context;

    public Decoder(Context context) {
        this.context = context;
    }

    public void decode(Uri resource, String culture, DecoderListener listener) throws IOException {
        try (InputStream resourceStream = context.getContentResolver().openInputStream(resource)) {
            try (MultipartUtility multipart = new MultipartUtility(DecodeURL, Charset)) {
                multipart.addFormField("culture", "culture");
                multipart.addFilePart("file", "audio", resourceStream);
                String response = multipart.finish(); // response from server.
                Log.v(TAG, response);
            }
        }
    }
}
