package glm.audiototext.ui;


import android.content.Context;
import android.net.Uri;
import android.os.AsyncTask;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;

import glm.audiototext.decoder.Decoder;
import glm.audiototext.decoder.DecoderListener;

public class RecognitionTask extends AsyncTask<Void, Integer, Void> {
    private final Context context;
    private final DecoderListener listener;
    private final String culture;
    private final Uri resource;

    RecognitionTask(Context context,
                    DecoderListener listener,
                    String culture,
                    Uri resource) {
        this.context = context;
        this.listener = listener;
        this.culture = culture;
        this.resource = resource;
    }

    @Override
    protected void onProgressUpdate(Integer... values) {
        super.onProgressUpdate(values);
    }

    @Override
    protected Void doInBackground(Void... params) {

        try {
            publishProgress(10);
            new Decoder(context).decode(this.resource, this.culture, this.listener);
        } catch (IOException e) {
            e.printStackTrace();
        }

        publishProgress(100);

        return null;
    }
}