import { definePreset } from '@primeng/themes';
import Material from '@primeng/themes/material';

const ThemePreset = definePreset(Material, {
  // PrimeNG customizations, see https://primeng.org/theming#customization for examples
  // Also view the node_modules/@primeng/themes/aura/base/index.ts to see a list of variables
  semantic: {
    primary: {
      50: '{blue.50}',
      100: '{blue.100}',
      200: '{blue.200}',
      300: '{blue.300}',
      400: '{blue.400}',
      500: '{blue.500}',
      600: '{blue.600}',
      700: '{blue.700}',
      800: '{blue.800}',
      900: '{blue.900}',
      950: '{blue.950}'
    },
    colorScheme: {
      light: {
        content: {
          background: '#FFFDD0'
        },
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
          950: '{gray.950}'
        },
        primary: {
          color: '#253749',
          hoverColor: '##768CA0',
          activeColor: '#253749'
        }
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
          950: '{gray.950}'
        },
        primary: {
          color: '#253749',
          hoverColor: '##768CA0',
          activeColor: '#253749'
        }
      }
    }
  },
  components: {
    button: {
      colorScheme: {
        dark: {
          text: {
            primary: {
              hoverBackground: '#395570',
              activeBackground: '1F2E3D',
              color: '#ffffff'
            }
          },
          root: {
            primary: {
              background: '#253749',
              borderColor: '#253749',
              color: '#ffffff',
              hoverBackground: '#395570',
              activeBackground: '#1F2E3D'
            }
          }
        }
      }
    },
    menubar: {
      root: {
        background: '#212124',
        color: '#ffffff',
        borderRadius: '0px',
        borderColor: 'transparent'
      },
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
          activeColor: '#ffffff'
        }
      },
      submenu: {
        background: '#212124',
        borderRadius: '0px'
      },
      mobileButton: {
        color: '{text.muted.color}',
        hoverColor: '{text.hover.muted.color}',
        hoverBackground: '#1F2E3D'
      }
    },
    drawer: {
      colorScheme: {
        dark: {
          root: {
            background: '#253749',
            borderColor: '#253749',
            color: '#ffffff'
          }
        },
        light: {
          root: {
            background: '#253749',
            borderColor: '#253749',
            color: '#ffffff'
          }
        }
      }
    },
    datatable: {
      headerCell: {
        background: '#253749',
        hoverBackground: '#768CA0',
        hoverColor: '#ffffff',
        borderColor: '#253749',
        color: '#ffffff',
        padding: '0.75rem 1rem'
      }
    },
    editor: {
      colorScheme: {
        dark: {
          content: {
            background: '#000000'
          },
          toolbar: {
            background: '#253749'
          },
          toolbarItem: {
            activeColor: '#ffffff',
            color: '#ffffff',
            hoverColor: '#768CA0'
          }
        },
        light: {
          content: {
            background: '#ffffff'
          },
          toolbar: {
            background: '#253749'
          },
          toolbarItem: {
            activeColor: '#ffffff',
            color: '#ffffff',
            hoverColor: '#768CA0'
          }
        }
      }
    },
    card: {
      root: {
        borderRadius: '0.5rem'
      },
      colorScheme: {
        dark: {
          root: {
            background: '#212124'
          }
        },
        light: {
          root: {
            background: '#eeeeee'
          }
        }
      }
    },
    toggleswitch: {
      colorScheme: {
        dark: {
          handle: {
            checkedBackground: '#009933'
          }
        },
        light: {
          handle: {
            checkedBackground: '#009933'
          }
        }
      }
    }
  }
});

export default ThemePreset;
