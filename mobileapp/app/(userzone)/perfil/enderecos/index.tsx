import { ThemedText } from '@/components/ui/ThemedText';
import { ThemedView } from '@/components/ui/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';

const AddressListScreen = () => {
	const router = useRouter();

	return (
		<ThemedView className="relative items-center justify-center flex-1 p-5">
			<ThemedView className="flex items-center justify-center flex-1 w-full gap-4" >
				<ThemedText>LISTA DE ENDEREÇOS</ThemedText>


			</ThemedView>

			<Button
				onPress={() => router.push('/(userzone)/perfil/enderecos/postalcode')}
				circular
				color="primary"
				className="!w-[40px] !h-[40px] absolute right-0 bottom-0 m-5"
			>
				<Ionicons name="add" color={'#FFFFFF'} />
			</Button>
		</ThemedView>
	);
};

export default AddressListScreen;