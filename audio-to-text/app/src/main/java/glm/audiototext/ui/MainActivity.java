package glm.audiototext.ui;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;

import java.lang.reflect.Array;
import java.util.Arrays;

import glm.audiototext.Preferences;
import glm.audiototext.R;

public class MainActivity extends AppCompatActivity {

    private Spinner activity_main_spnLanguages;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        this.activity_main_spnLanguages = findViewById(R.id.activity_main_spnLanguages);

        loadLanguages();
    }

    private void loadLanguages() {
        final String[] languages = getResources().getStringArray(R.array.supported_decode_languages);
        ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, languages);
        this.activity_main_spnLanguages.setAdapter(adapter);
        int index = Arrays.asList(languages).indexOf(Preferences.getLanguage(this, languages));
        this.activity_main_spnLanguages.setSelection(index);
        this.activity_main_spnLanguages.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                Preferences.setLanguage(MainActivity.this, languages[position]);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
    }

}
