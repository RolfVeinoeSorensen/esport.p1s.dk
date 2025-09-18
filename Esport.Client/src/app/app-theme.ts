import { definePreset } from '@primeng/themes';
import Aura from '@primeng/themes/aura';

const ThemePreset = definePreset(Aura, {
  // PrimeNG customizations, see https://primeng.org/theming#customization for examples
  // Also view the node_modules/@primeng/themes/aura/base/index.ts to see a list of variables
  semantic: {
    borderRadius: {
      none: '0px',
      xs: '2px',
      sm: '4px',
      md: '4px',
      lg: '6px',
      xl: '10px',
    },
    colorScheme: {
      light: {
        surface: {
          0: '#ffffff',
          50: '{gray.50}',
          100: '{gray.100}',
          200: '{gray.200}',
          300: '{gray.300}',
          400: '{gray.400}',
          500: '{gray.500}',
          600: '{gray.600}',
          700: '{gray.700}',
          800: '{gray.800}',
          900: '{gray.900}',
          950: '{gray.950}',
        },
        primary: {
          color: '#253749',
          inverseColor: '#ffffff',
          hoverColor: '##768CA0',
          activeColor: '#253749',
          50: '{gray.50}',
          100: '{gray.100}',
          200: '{gray.200}',
          300: '{gray.300}',
          400: '{gray.400}',
          500: '{gray.500}',
          600: '{gray.600}',
          700: '{gray.700}',
          800: '{gray.800}',
          900: '{gray.900}',
          950: '{gray.950}',
        },
      },
      dark: {
        surface: {
          0: '#ffffff',
          50: '{gray.50}',
          100: '{gray.100}',
          200: '{gray.200}',
          300: '{gray.300}',
          400: '{gray.400}',
          500: '{gray.500}',
          600: '{gray.600}',
          700: '{gray.700}',
          800: '{gray.800}',
          900: '{gray.900}',
          950: '{gray.950}',
        },
        primary: {
          color: '#253749',
          inverseColor: '#ffffff',
          hoverColor: '##768CA0',
          activeColor: '#253749',
          50: '{gray.50}',
          100: '{gray.100}',
          200: '{gray.200}',
          300: '{gray.300}',
          400: '{gray.400}',
          500: '{gray.500}',
          600: '{gray.600}',
          700: '{gray.700}',
          800: '{gray.800}',
          900: '{gray.900}',
          950: '{gray.950}',
        },
      },
    },
  },
  components: {
    menubar: {
      background: '#253749',
      color: '#ffffff',
      borderRadius: '0px',
      borderColor: 'transparent',
      item: {
        focusBackground: '#395570',
        activeBackground: '#1F2E3D',
        color: '#ffffff',
        focusColor: '#ffffff',
        activeColor: '#ffffff',
        padding: '{navigation.item.padding}',
        borderRadius: '0px',
        gap: '{navigation.item.gap}',
        icon: {
          color: '#ffffff',
          focusColor: '#ffffff',
          activeColor: '#ffffff',
        },
      },
      submenu: {
        background: '#253749',
        color: '#ffffff',
        borderRadius: '0px',
      },
      mobileButton: {
        color: '{text.muted.color}',
        hoverColor: '{text.hover.muted.color}',
        hoverBackground: '#1F2E3D',
      }
    },
    drawer: {
      background: '#253749',
      borderRadius: '0px',
      borderColor: '#253749',
      color: '#ffffff',
    },
    datatable: {
      headerCell: {
        background: '#253749',
        hoverBackground: '##768CA0',
        hoverColor: '#ffffff',
        borderColor: '#253749',
        color: '#ffffff',
        borderWidth: '0 0 1px 0',
        padding: '0.75rem 1rem',
      },
    },
  },
});

export default ThemePreset;
