package glm.audiototext;

import android.content.Context;
import android.content.SharedPreferences;

import java.util.Arrays;
import java.util.Locale;

public final class Preferences {

    private static final String PreferencesName = "Preferences";
    private static final String KeyDecodeCulture = "DecodeCulture";

    private Preferences() { }

    public static String getLanguage(Context context, String[] allowed) {
        Locale locale = Locale.getDefault();
        String defaultCultureCode = locale.getLanguage() + "-" + locale.getCountry().toUpperCase();

        if (Arrays.asList(allowed).indexOf(defaultCultureCode) < 0)
            defaultCultureCode = allowed[0];

        return context.getSharedPreferences(PreferencesName, Context.MODE_PRIVATE)
                .getString(KeyDecodeCulture, defaultCultureCode);
    }

    public static void setLanguage(Context context, String value) {
        SharedPreferences.Editor editor = context.getSharedPreferences(PreferencesName, Context.MODE_PRIVATE).edit();

        editor.putString(KeyDecodeCulture, value);
        editor.apply();
    }
}
