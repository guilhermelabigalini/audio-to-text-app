package glm.audiototext.ui;

import android.content.ClipData;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ProgressBar;

import glm.audiototext.Preferences;
import glm.audiototext.R;
import glm.audiototext.decoder.DecoderListener;

public class ReceiveAudioActivity extends AppCompatActivity implements DecoderListener {
    @Override
    public void onPartialResult(String message) {

    }

    @Override
    public void onFailed(String message) {

    }

    public enum FinalResponseStatus { NotReceived, OK, Timeout }

    public static final String LOG = "ReceiveAudioActivity";
    FinalResponseStatus isReceivedResponse = FinalResponseStatus.NotReceived;


    private EditText activity_receive_edtDecoded;
    private ProgressBar activity_receive_pgBarConvert;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_receive_audio);


        this.activity_receive_edtDecoded = findViewById(R.id.activity_receive_edtDecoded);
        this.activity_receive_pgBarConvert = findViewById(R.id.activity_receive_pgBarConvert);
        
        Intent intent = getIntent();

        if (intent != null) {
            String type = intent.getType();
            String action = intent.getAction();

            Uri resource;

            Log.v(LOG, "type =" + type);
            Log.v(LOG, "action =" + action);

            if (intent.getData() != null) {
                Log.v(LOG, "using intent.getData()");

                resource = intent.getData();

            } else  {
                Log.v(LOG, "data =null");
                Log.v(LOG, "clip-data =" + intent.getClipData().toString());

                ClipData.Item item = intent.getClipData().getItemAt(0);

                // Tries to get the item's contents as a URI pointing to a note
                resource = item.getUri();
            }

            decodeToText(resource);

        } else {
            Log.v(LOG, "intent =null");
            finish();
        }
    }

    private String getCulture() {
        final String[] languages = getResources().getStringArray(R.array.supported_decode_languages);
        return Preferences.getLanguage(this, languages);
    }

    private void setConvertisionProgress(boolean show) {
        if (show)
            this.activity_receive_pgBarConvert.setVisibility(View.VISIBLE);
        else
            this.activity_receive_pgBarConvert.setVisibility(View.GONE);
    }

    private void decodeToText(Uri resource) {
        Log.v(LOG, "decodeToText, resource = " + resource.toString());

        setConvertisionProgress(true);

        new RecognitionTask(this, this, getCulture(), resource) {
            @Override
            protected void onPostExecute(Void aVoid) {
                setConvertisionProgress(false);
            }

            @Override
            protected void onCancelled() {
                setConvertisionProgress(false);
            }
        };
    }

    private void WriteLine() {
        this.WriteLine("");
    }

    private void WriteLine(String text) {
        this.activity_receive_edtDecoded.append(text + "\n");
    }
/*
    @Override
    public void onPartialResponseReceived(String response) {
        this.WriteLine("--- Partial result received by onPartialResponseReceived() ---");
        this.WriteLine(response);
        this.WriteLine();
    }

    @Override
    public void onFinalResponseReceived(RecognitionResult response) {
        boolean isFinalDicationMessage = this.getMode() == SpeechRecognitionMode.LongDictation &&
                (response.RecognitionStatus == RecognitionStatus.EndOfDictation ||
                        response.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout);

        if (isFinalDicationMessage) {
            this.isReceivedResponse = FinalResponseStatus.OK;
        }

        if (!isFinalDicationMessage) {
            this.WriteLine("********* Final n-BEST Results *********");
            for (int i = 0; i < response.Results.length; i++) {
                this.WriteLine("[" + i + "]" + " Confidence=" + response.Results[i].Confidence +
                        " Text=\"" + response.Results[i].DisplayText + "\"");
            }

            this.WriteLine();
        }
    }*/
}
