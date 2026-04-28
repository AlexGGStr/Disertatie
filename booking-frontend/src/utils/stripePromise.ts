import { loadStripe } from '@stripe/stripe-js';
import {stripePublishableKey} from "../config";

// Use your real publishable key from appsettings or environment
export const stripePromise = loadStripe(stripePublishableKey);
